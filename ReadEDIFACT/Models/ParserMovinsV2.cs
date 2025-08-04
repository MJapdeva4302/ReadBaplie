using System;
using System.Collections.Generic;
using System.Linq;

namespace ReadEDIFACT.Models
{
    public class MovinsV2Data
    {
        public string SenderIdentification { get; set; } = "";
        public string ReceiverIdentification { get; set; } = "";
        public VesselInfo VesselInformation { get; set; } = new VesselInfo();
        public List<ContainerInfo> Containers { get; set; } = new List<ContainerInfo>();
    }

    public class ParserMovinsV2 : BaseEDIParser
    {
        private readonly FileDefinition _movinsDefinition;
        private MovinsV2Data? _data;

        private readonly HashSet<string> ValidStatusCodes = new HashSet<string>
        {
            "AV",   // Available
            "DM",   // Damaged
            "MT",   // Empty
            "LD",   // Loaded
            "RE",   // Reserved
            "BK"    // Booked
        };

        public ParserMovinsV2()
        {
            _movinsDefinition = new MovinsVersion2();
        }

        public static MovinsV2Data? LoadFromJson(string filePath)
        {
            return LoadJson<MovinsV2Data>(filePath);
        }

        public (bool isValid, List<string> errors) ValidateMandatoryFields(MovinsV2Data data)
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

            // Validar contenedores - Reglas específicas de MOVINS v2
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
                    
                    // Validaciones específicas de MOVINS v2
                    if (string.IsNullOrWhiteSpace(container.Status))
                        errors.Add($"Estado del contenedor {container.ContainerNumber} es obligatorio");
                    else if (!ValidStatusCodes.Contains(container.Status))
                        errors.Add($"Estado del contenedor {container.ContainerNumber} inválido. Estados válidos: {string.Join(", ", ValidStatusCodes)}");

                    if (!string.IsNullOrWhiteSpace(container.Weight))
                    {
                        if (!ValidateWeight(container.Weight))
                            errors.Add($"Peso inválido para el contenedor {container.ContainerNumber}");
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

        public string GenerateEDI(string senderIdentification, string receiverIdentification, List<ContainerInfo> containers, VesselInfo vesselInfo)
        {
            // Crear objeto de datos para validación
            var data = new MovinsV2Data
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
                throw new InvalidOperationException($"Datos inválidos para generar EDI MOVINS V2:\n{string.Join("\n", validationErrors)}");
            }

            var segments = new List<SegmentData>();
            var interchangeControl = GenerateInterchangeRef();
            var messageRefNumber = GenerateMessageRefNumber();

            // Add UNB
            segments.Add(CreateUnbSegment(senderIdentification, receiverIdentification, interchangeControl));

            // Add UNH - MOVINS version 2
            segments.Add(CreateUnhSegment("MOVINS", "D", "95B", messageRefNumber));

            // Add BGM - Beginning of Message
            segments.Add(new SegmentData
            {
                SegmentID = "BGM",
                Usage = RuleUsage.Mandatory,
                DataElements = new Element[]
                {
                    new DataElement { Name = "Document Name Code", Usage = RuleUsage.Mandatory,
                            DataType = DataType.Alphanumeric, Value = "312" },
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
                // EQD - Equipment Details
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
                        },
                        new DataElement { Name = "Status", Usage = RuleUsage.Required,
                                DataType = DataType.Alphanumeric, Value = container.Status }
                    }
                });

                // LOC - Location
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

                // MEA - Measurements
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
            }

            // Add UNT
            segments.Add(CreateUntSegment(segments.Count + 1, messageRefNumber));

            // Add UNZ
            segments.Add(CreateUnzSegment(interchangeControl));

            return string.Join("'", segments.Select(s => s.ToString()));
        }
    }
}
