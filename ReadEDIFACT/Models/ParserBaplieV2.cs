using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace ReadEDIFACT.Models
{
    public class ParserBaplieV2
    {
        private List<BaplieV2Equipment>? _equipments;
        private BaplieV2ArrivalData? _arrivalData;
        private readonly FileDefinition _baplieDefinition;

        public ParserBaplieV2(BaplieV2ArrivalData arrivalData, List<BaplieV2Equipment> equipments)
        {
            _arrivalData = arrivalData;
            _equipments = equipments;
            _baplieDefinition = new BaplieVersion2();
        }

        private static string GenerateInterchangeRef()
        {
            string timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
            if (timestamp.Length > 10)
            {
                timestamp = timestamp.Substring(timestamp.Length - 10);
            }
            Random random = new Random();
            string randomNumber = random.Next(1000, 9999).ToString();
            string interchangeRef = $"{timestamp}{randomNumber}";
            if (interchangeRef.Length > 14)
            {
                interchangeRef = interchangeRef.Substring(0, 14);
            }
            return interchangeRef;
        }

        private static string GenerateMessageRefNumber()
        {
            string timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
            if (timestamp.Length > 5)
            {
                timestamp = timestamp.Substring(timestamp.Length - 5);
            }
            Random random = new Random();
            string randomNumber = random.Next(100, 999).ToString();
            string messageRef = $"{timestamp}{randomNumber}";
            if (messageRef.Length > 8)
            {
                messageRef = messageRef.Substring(0, 8);
            }
            return messageRef;
        }

        public static BaplieV2RootData? LoadJson(string filePath)
        {
            try
            {
                string json = File.ReadAllText(filePath);
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                var result = JsonSerializer.Deserialize<BaplieV2RootData>(json, options);
                if (result == null)
                {
                    throw new InvalidOperationException("Failed to deserialize JSON to BaplieV2RootData.");
                }
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading JSON: {ex.Message}");
                return null;
            }
        }
    {
        private readonly FileDefinition _baplieDefinition;
        private BaplieV2Data? _data;

        public ParserBaplieV2()
        {
            _baplieDefinition = new BaplieVersion2();
        }

        public static BaplieV2Data? LoadFromJson(string filePath)
        {
            return LoadJson<BaplieV2Data>(filePath);
        }

        public (bool isValid, List<string> errors) ValidateMandatoryFields(BaplieV2Data data)
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
            }

            // Validar contenedores
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
                }
            }

            return (!errors.Any(), errors);
        }

        private bool ValidateContainerNumber(string containerNumber)
        {
            // Implementar validación de número de contenedor según ISO 6346
            // Formato: 4 letras (código del propietario y categoría) + 6 números + 1 dígito de verificación
            if (containerNumber.Length != 11)
                return false;

            var ownerCode = containerNumber.Substring(0, 4);
            var serialNumber = containerNumber.Substring(4, 6);
            var checkDigit = containerNumber.Substring(10, 1);

            // Verificar que los primeros 4 caracteres son letras
            if (!ownerCode.All(char.IsLetter))
                return false;

            // Verificar que los siguientes 6 caracteres son números
            if (!serialNumber.All(char.IsDigit))
                return false;

            // Verificar que el último carácter es un número
            if (!checkDigit.All(char.IsDigit))
                return false;

            return true;
        }

        public string GenerateEDI(string senderIdentification, string receiverIdentification, List<ContainerInfo> containers, VesselInfo vesselInfo)
        {
            // Crear objeto de datos para validación
            var data = new BaplieV2Data
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
                throw new InvalidOperationException($"Datos inválidos para generar EDI BAPLIE:\n{string.Join("\n", validationErrors)}");
            }

            var segments = new List<SegmentData>();
            var interchangeControl = GenerateInterchangeRef();
            var messageRefNumber = GenerateMessageRefNumber();

            // Add UNB
            segments.Add(CreateUnbSegment(senderIdentification, receiverIdentification, interchangeControl));

            // Add UNH
            segments.Add(CreateUnhSegment("BAPLIE", "D", "95B", messageRefNumber));

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
            }

            // Add UNT
            segments.Add(CreateUntSegment(segments.Count + 1, messageRefNumber));

            // Add UNZ
            segments.Add(CreateUnzSegment(interchangeControl));

            return string.Join("'", segments.Select(s => s.ToString()));
        }
    }
}
