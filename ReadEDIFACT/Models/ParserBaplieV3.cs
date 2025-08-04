using System;
using System.Collections.Generic;
using System.Linq;

namespace ReadEDIFACT.Models
{
    public class BaplieV3Data
    {
        public string SenderIdentification { get; set; } = "";
        public string ReceiverIdentification { get; set; } = "";
        public VesselInfo VesselInformation { get; set; } = new VesselInfo();
        public List<ContainerInfo> Containers { get; set; } = new List<ContainerInfo>();
    }

    public class ParserBaplieV3 : BaseEDIParser
    {
        private readonly FileDefinition _baplieDefinition;
        private BaplieV3Data? _data;

        public ParserBaplieV3()
        {
            _baplieDefinition = new BaplieVersion3();
        }

        public static BaplieV3Data? LoadFromJson(string filePath)
        {
            return LoadJson<BaplieV3Data>(filePath);
        }

        public (bool isValid, List<string> errors) ValidateMandatoryFields(BaplieV3Data data)
        {
            var errors = new List<string>();

            // Validar campos obligatorios generales
            if (string.IsNullOrWhiteSpace(data.SenderIdentification))
                errors.Add("El campo SenderIdentification es obligatorio");
            if (string.IsNullOrWhiteSpace(data.ReceiverIdentification))
                errors.Add("El campo ReceiverIdentification es obligatorio");

            // Validar información del buque
            if (data.VesselInformation == null)
                errors.Add("La información del buque es obligatoria");
            else
            {
                if (string.IsNullOrWhiteSpace(data.VesselInformation.VesselName))
                    errors.Add("El nombre del buque es obligatorio");
                if (string.IsNullOrWhiteSpace(data.VesselInformation.VoyageNumber))
                    errors.Add("El número de viaje es obligatorio");
                if (string.IsNullOrWhiteSpace(data.VesselInformation.VesselOperator))
                    errors.Add("El operador del buque es obligatorio");
            }

            // Validar contenedores - Reglas específicas de BAPLIE v3
            if (data.Containers == null || !data.Containers.Any())
                errors.Add("Se requiere al menos un contenedor");
            else
            {
                foreach (var container in data.Containers)
                {
                    if (string.IsNullOrWhiteSpace(container.ContainerNumber))
                        errors.Add($"Número de contenedor es obligatorio");
                    else if (!ValidateContainerNumber(container.ContainerNumber))
                        errors.Add($"Número de contenedor inválido: {container.ContainerNumber}");

                    if (string.IsNullOrWhiteSpace(container.Location))
                        errors.Add($"Posición del contenedor {container.ContainerNumber} es obligatoria");
                    if (string.IsNullOrWhiteSpace(container.Type))
                        errors.Add($"Tipo de contenedor {container.ContainerNumber} es obligatorio");
                    if (string.IsNullOrWhiteSpace(container.Size))
                        errors.Add($"Tamaño del contenedor {container.ContainerNumber} es obligatorio");
                    
                    // Validaciones adicionales específicas de BAPLIE v3
                    if (!string.IsNullOrWhiteSpace(container.Weight) && !ValidateWeight(container.Weight))
                        errors.Add($"Peso inválido para el contenedor {container.ContainerNumber}");
                    
                    if (!string.IsNullOrWhiteSpace(container.PortOfLoading))
                    {
                        if (!ValidatePortCode(container.PortOfLoading))
                            errors.Add($"Código de puerto de carga inválido para el contenedor {container.ContainerNumber}");
                    }
                    
                    if (!string.IsNullOrWhiteSpace(container.PortOfDischarge))
                    {
                        if (!ValidatePortCode(container.PortOfDischarge))
                            errors.Add($"Código de puerto de descarga inválido para el contenedor {container.ContainerNumber}");
                    }
                }
            }

            return (!errors.Any(), errors);
        }

        private bool ValidateContainerNumber(string containerNumber)
        {
            // Implementar validación de número de contenedor según ISO 6346
            if (containerNumber.Length != 11)
                return false;

            var ownerCode = containerNumber.Substring(0, 4);
            var serialNumber = containerNumber.Substring(4, 6);
            var checkDigit = containerNumber.Substring(10, 1);

            if (!ownerCode.All(char.IsLetter))
                return false;

            if (!serialNumber.All(char.IsDigit))
                return false;

            if (!checkDigit.All(char.IsDigit))
                return false;

            return true;
        }

        private bool ValidateWeight(string weight)
        {
            // El peso debe ser un número válido mayor que 0
            if (!decimal.TryParse(weight, out decimal weightValue))
                return false;

            return weightValue > 0;
        }

        private bool ValidatePortCode(string portCode)
        {
            // Validar código de puerto según UN/LOCODE
            // Formato: 5 caracteres, primeros 2 son el código ISO del país
            if (portCode.Length != 5)
                return false;

            var countryCode = portCode.Substring(0, 2);
            var locationCode = portCode.Substring(2);

            // Los primeros dos caracteres deben ser letras (código de país)
            if (!countryCode.All(char.IsLetter))
                return false;

            // Los siguientes tres caracteres deben ser letras o números
            if (!locationCode.All(c => char.IsLetterOrDigit(c)))
                return false;

            return true;
        }

        public string GenerateEDI(string senderIdentification, string receiverIdentification, List<ContainerInfo> containers, VesselInfo vesselInfo)
        {
            // Crear objeto de datos para validación
            var data = new BaplieV3Data
            {
                SenderIdentification = senderIdentification,
                ReceiverIdentification = receiverIdentification,
                VesselInformation = vesselInfo,
                Containers = containers
            };

            // Validar los datos antes de generar el EDI
            var (isValid, validationErrors) = ValidateMandatoryFields(data);
            if (!isValid)
            {
                throw new InvalidOperationException($"Datos inválidos para generar EDI BAPLIE V3:\n{string.Join("\n", validationErrors)}");
            }

            var segments = new List<SegmentData>();
            var interchangeControl = GenerateInterchangeRef();
            var messageRefNumber = GenerateMessageRefNumber();

            // Add UNB
            segments.Add(CreateUnbSegment(senderIdentification, receiverIdentification, interchangeControl));

            // Add UNH - Note the different version for BAPLIE v3
            segments.Add(CreateUnhSegment("BAPLIE", "D", "00B", messageRefNumber));

            // Add BGM - Beginning of Message
            segments.Add(new SegmentData
            {
                SegmentID = "BGM",
                Usage = RuleUsage.Mandatory,
                DataElements = new Element[]
                {
                    new DataElement { Name = "Document Name Code", Usage = RuleUsage.Mandatory,
                            DataType = DataType.Alphanumeric, Value = "172" },
                    new DataElement { Name = "Document Number", Usage = RuleUsage.Required,
                            DataType = DataType.Alphanumeric, Value = messageRefNumber }
                }
            });

            // Add TDT for vessel information
            segments.Add(new SegmentData
            {
                SegmentID = "TDT",
                Usage = RuleUsage.Mandatory,
                DataElements = new Element[]
                {
                    new DataElement { Name = "Transport Stage Qualifier", Usage = RuleUsage.Mandatory,
                            DataType = DataType.Alphanumeric, Value = "20" },
                    new DataElement { Name = "Vessel Voyage", Usage = RuleUsage.Required,
                            DataType = DataType.Alphanumeric, Value = vesselInfo.VoyageNumber },
                    new EmptyElement(),
                    new EmptyElement(),
                    new CompositeElement
                    {
                        DataElements = new DataElement[]
                        {
                            new DataElement { Name = "Vessel Name", Usage = RuleUsage.Required,
                                    DataType = DataType.Alphanumeric, Value = vesselInfo.VesselName },
                            new DataElement { Name = "Vessel Operator", Usage = RuleUsage.Required,
                                    DataType = DataType.Alphanumeric, Value = vesselInfo.VesselOperator }
                        }
                    }
                }
            });

            // Add container information
            foreach (var container in containers)
            {
                segments.Add(new SegmentData
                {
                    SegmentID = "LOC",
                    Usage = RuleUsage.Mandatory,
                    DataElements = new Element[]
                    {
                        new DataElement { Name = "Location Function", Usage = RuleUsage.Mandatory,
                                DataType = DataType.Alphanumeric, Value = "147" },
                        new CompositeElement
                        {
                            DataElements = new DataElement[]
                            {
                                new DataElement { Name = "Location", Usage = RuleUsage.Required,
                                        DataType = DataType.Alphanumeric, Value = container.Location }
                            }
                        }
                    }
                });

                segments.Add(new SegmentData
                {
                    SegmentID = "EQD",
                    Usage = RuleUsage.Mandatory,
                    DataElements = new Element[]
                    {
                        new DataElement { Name = "Equipment Type", Usage = RuleUsage.Mandatory,
                                DataType = DataType.Alphanumeric, Value = "CN" },
                        new DataElement { Name = "Container Number", Usage = RuleUsage.Required,
                                DataType = DataType.Alphanumeric, Value = container.ContainerNumber },
                        new CompositeElement
                        {
                            DataElements = new DataElement[]
                            {
                                new DataElement { Name = "Size and Type", Usage = RuleUsage.Required,
                                        DataType = DataType.Alphanumeric, Value = container.Size },
                                new DataElement { Name = "Container Type", Usage = RuleUsage.Required,
                                        DataType = DataType.Alphanumeric, Value = container.Type }
                            }
                        }
                    }
                });

                // Add weight information if available
                if (!string.IsNullOrEmpty(container.Weight))
                {
                    segments.Add(new SegmentData
                    {
                        SegmentID = "MEA",
                        Usage = RuleUsage.Conditional,
                        DataElements = new Element[]
                        {
                            new DataElement { Name = "Measurement Purpose", Usage = RuleUsage.Mandatory,
                                    DataType = DataType.Alphanumeric, Value = "AAE" },
                            new DataElement { Name = "Measurement Value", Usage = RuleUsage.Required,
                                    DataType = DataType.Alphanumeric, Value = container.Weight },
                            new DataElement { Name = "Unit of Measure", Usage = RuleUsage.Required,
                                    DataType = DataType.Alphanumeric, Value = "KGM" }
                        }
                    });
                }

                // Add port information
                if (!string.IsNullOrEmpty(container.PortOfLoading))
                {
                    segments.Add(new SegmentData
                    {
                        SegmentID = "LOC",
                        Usage = RuleUsage.Conditional,
                        DataElements = new Element[]
                        {
                            new DataElement { Name = "Location Function", Usage = RuleUsage.Mandatory,
                                    DataType = DataType.Alphanumeric, Value = "9" },
                            new DataElement { Name = "Port of Loading", Usage = RuleUsage.Required,
                                    DataType = DataType.Alphanumeric, Value = container.PortOfLoading }
                        }
                    });
                }

                if (!string.IsNullOrEmpty(container.PortOfDischarge))
                {
                    segments.Add(new SegmentData
                    {
                        SegmentID = "LOC",
                        Usage = RuleUsage.Conditional,
                        DataElements = new Element[]
                        {
                            new DataElement { Name = "Location Function", Usage = RuleUsage.Mandatory,
                                    DataType = DataType.Alphanumeric, Value = "11" },
                            new DataElement { Name = "Port of Discharge", Usage = RuleUsage.Required,
                                    DataType = DataType.Alphanumeric, Value = container.PortOfDischarge }
                        }
                    });
                }
            }

            // Add UNT
            segments.Add(CreateUntSegment(segments.Count + 1, messageRefNumber));

            // Add UNZ
            segments.Add(CreateUnzSegment(interchangeControl));

            return string.Join("'", segments.Select(s => s.ToString()));
        }
    }
}
