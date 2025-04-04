using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using System.Text.Json;
using Newtonsoft.Json;

namespace ReadEDIFACT.Models.Coarri
{
    public class ParserCOARRI
    {
        private List<Equipment>? _equipments;
        private ArrivalData? _arrivalData;

        public ParserCOARRI(ArrivalData arrivalData, List<Equipment> equipments)
        {
            _arrivalData = arrivalData;
            _equipments = equipments;
        }

        public static RootData? LoadJson(string filePath)
        {
            try
            {
                string json = File.ReadAllText(filePath);
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                var result = System.Text.Json.JsonSerializer.Deserialize<RootData>(json, options);
                if (result == null)
                {
                    throw new InvalidOperationException("Failed to deserialize JSON to RootData.");
                }
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading JSON: {ex.Message}");
                return null;
            }
        }

        // Método para generar un número único de referencia
        public static string GenerateInterchangeRef()
        {
            string timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmss");

            if (timestamp.Length > 10)
            {
                timestamp = timestamp.Substring(timestamp.Length - 10);
            }

            Random random = new Random();
            // Número entre 1000 y 9999
            string randomNumber = random.Next(1000, 9999).ToString();


            string interchangeRef = $"{timestamp}{randomNumber}";


            if (interchangeRef.Length > 14)
            {
                interchangeRef = interchangeRef.Substring(0, 14);
            }

            return interchangeRef;
        }

        // Método para generar un número de referencia de mensaje unico para el segmento UNH
        public static string GenerateMessageRefNumber()
        {
            string timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmss");

            if (timestamp.Length > 5)
            {
                timestamp = timestamp.Substring(timestamp.Length - 5);
            }

            Random random = new Random();

            string randomNumber = random.Next(100, 999).ToString();


            string interchangeRef = $"{timestamp}{randomNumber}";


            if (interchangeRef.Length > 8)
            {
                interchangeRef = interchangeRef.Substring(0, 8);
            }

            return interchangeRef;
        }

        public static ArrivalData MapArrivalDataFromJson(ArrivalDataJson jsonData)
        {
            var arrivalData = new ArrivalData();
            var InterchangeControl = GenerateInterchangeRef();
            var GenerateMessageReferenceNumber = GenerateMessageRefNumber();

            // UNB - Interchange Header
            arrivalData.InterchangeHeader = new SegmentData
            {
                SegmentID = "UNB",
                Usage = RuleUsage.Mandatory,
                DataElements = new Element[]
                {
                    new CompositeElement
                    {
                        DataElements = new DataElement[]
                        {
                            new DataElement { Name = "Syntax Identifier", Usage = RuleUsage.Mandatory,
                                    DataType = DataType.Alphabetic, Value = "UNOA" },
                            new DataElement { Name = "Syntax Version", Usage = RuleUsage.Mandatory,
                                    DataType = DataType.Numeric, Value = "2" }
                        }
                    },
                    new DataElement { Name = "Sender Identification", Usage = RuleUsage.Mandatory,
                            DataType = DataType.Alphanumeric, Value = "JAPDEVACRMOB" },
                    new DataElement { Name = "Receiver Identification", Usage = RuleUsage.Mandatory,
                            DataType = DataType.Alphanumeric, Value = "CRCUSTOMS01" },
                    new CompositeElement
                    {
                        DataElements = new DataElement[]
                        {
                            new DataElement { Name = "Date", Usage = RuleUsage.Mandatory,
                                    DataType = DataType.Numeric, Value = DateTime.UtcNow.ToString("yyMMdd") },
                            new DataElement { Name = "Time", Usage = RuleUsage.Mandatory,
                                    DataType = DataType.Numeric, Value = DateTime.UtcNow.ToString("HHmm") }
                        }
                    },
                    new DataElement { Name = "Interchange Control Reference", Usage = RuleUsage.Mandatory,
                            DataType = DataType.Alphanumeric, Value = InterchangeControl },
                }
            };

            // UNH - Message Header
            arrivalData.MessageHeader = new SegmentData
            {
                SegmentID = "UNH",
                Usage = RuleUsage.Mandatory,
                DataElements = new Element[]
                {
                    new DataElement { Name = "Message Reference Number", Usage = RuleUsage.Mandatory,
                            DataType = DataType.Alphanumeric, Value = GenerateMessageReferenceNumber },
                    new CompositeElement
                    {
                        DataElements = new DataElement[]
                        {
                            new DataElement { Name = "Message Type Identifier", Usage = RuleUsage.Mandatory,
                                    DataType = DataType.Alphanumeric, Value = "COARRI" },
                            new DataElement { Name = "Message Type Version", Usage = RuleUsage.Mandatory,
                                    DataType = DataType.Alphanumeric, Value = "D" },
                            new DataElement { Name = "Message Release Number", Usage = RuleUsage.Mandatory,
                                    DataType = DataType.Alphanumeric, Value = "23A" },
                            new DataElement { Name = "Controlling Agency", Usage = RuleUsage.Mandatory,
                                    DataType = DataType.Alphanumeric, Value = "UN" },
                            new DataElement { Name = "Association Assigned Code", Usage = RuleUsage.Required,
                                    DataType = DataType.Alphanumeric, Value = "ITG10" }
                        }
                    }
                }
            };

            // BGM - Beginning of Message
            arrivalData.BeginningOfMessage = new SegmentData
            {
                SegmentID = "BGM",
                Usage = RuleUsage.Mandatory,
                DataElements = new Element[]
                {
                    new DataElement { Name = "Document Name", Usage = RuleUsage.Required,
                            DataType = DataType.Alphanumeric, Value = jsonData.DocNameCode.ToString() },
                    new DataElement { Name = "Document Number", Usage = RuleUsage.Required,
                            DataType = DataType.Alphanumeric, Value = "" },
                    new DataElement { Name = "Message Function", Usage = RuleUsage.Required,
                            DataType = DataType.Alphanumeric, Value = "9" }
                }
            };

            // TDT - Transport Information
            arrivalData.TransportInformation = new SegmentData
            {
                SegmentID = "TDT",
                Usage = RuleUsage.Mandatory,
                DataElements = new Element[]
                {
                    new DataElement { Name = "Transport Stage Qualifier", Usage = RuleUsage.Mandatory,
                                    DataType = DataType.Alphanumeric, Value = "20" },
                    new DataElement { Name = "Conveyance Reference Number", Usage = RuleUsage.Required,
                                    DataType = DataType.Alphanumeric, Value = "133" },
                    new DataElement { Name = "Transport mode name code", Usage = RuleUsage.Conditional,
                                    DataType = DataType.Alphanumeric, Value = "1" },
                    new EmptyElement(),
                    new CompositeElement
                    {
                        DataElements = new DataElement[]
                        {
                            new DataElement { Name = "Carrier identifier", Usage = RuleUsage.Required,
                                            DataType = DataType.Alphanumeric, Value = "" },
                            new DataElement { Name = "Code list responsible agency, coded", Usage = RuleUsage.Required,
                                            DataType = DataType.Alphanumeric, Value = "ZZZ" },
                            new DataElement { Name = "Code List Qualifier", Usage = RuleUsage.Required,
                                            DataType = DataType.Alphanumeric, Value = "172" },
                            new DataElement { Name = "Code list responsible agency, coded", Usage = RuleUsage.Required,
                                            DataType = DataType.Alphanumeric, Value = "166" },
                            new DataElement { Name = "Carrier Name", Usage = RuleUsage.Required,
                                            DataType = DataType.Alphanumeric, Value = jsonData.ShipName }
                        }
                    },
                    new EmptyElement(),
                    new EmptyElement(),
                    new CompositeElement
                    {
                        DataElements = new DataElement[]
                        {
                            new DataElement { Name = "Id of Means of Transport Identification", Usage = RuleUsage.Required,
                                            DataType = DataType.Alphanumeric, Value = jsonData.IMO.ToString() },
                            new DataElement { Name = "Code List Qualifier", Usage = RuleUsage.Required,
                                            DataType = DataType.Alphanumeric, Value = "146" },
                            new DataElement { Name = "Code list responsible agency, coded", Usage = RuleUsage.Required,
                                            DataType = DataType.Alphanumeric, Value = "11" },
                            new DataElement { Name = "Name of Means of Transport Identification", Usage = RuleUsage.Required,
                                            DataType = DataType.Alphanumeric, Value = jsonData.ShipName }
                        }
                    }
                }
            };

            // RFF - Reference
            arrivalData.Reference = new SegmentData
            {
                SegmentID = "RFF",
                DataElements = new Element[]
                {
                    new CompositeElement
                    {
                        DataElements = new DataElement[]
                        {
                            new DataElement { Name = "Reference Qualifier", Usage = RuleUsage.Mandatory,
                                    DataType = DataType.Alphanumeric, Value = "VM" },
                            new DataElement { Name = "Reference Number", Usage = RuleUsage.Required,
                                    DataType = DataType.Alphanumeric, Value = jsonData.CallSign ?? "" }
                        }
                    }
                }
            };

            // LOC - Locations
            arrivalData.Locations = new List<SegmentData>
            {
                CreateLocationSegment(
                    jsonData.DocNameCode == 119 ? "41" : "42",  // 119=Descarga, 122=Carga
                    jsonData.DischargePortCode,
                    jsonData.DocNameCode.ToString()),
                CreateLocationSegment("9", jsonData.LoadingPortCode, jsonData.DocNameCode.ToString(), jsonData.LoadingPortName),
                CreateLocationSegment("94", jsonData.OriginPort, jsonData.DocNameCode.ToString(), jsonData.OriginPortName),
                CreateLocationSegment("11", jsonData.DischargePortCode, jsonData.DocNameCode.ToString(), jsonData.DischargePortName),
                CreateLocationSegment("61", jsonData.DestinationPort, jsonData.DocNameCode.ToString(), jsonData.DestinationPortName)
            };

            // DTM - Date/Time
            arrivalData.DateTimes = new List<SegmentData>
            {
                CreateDateTimeSegment("132", jsonData.ETA, "203", jsonData.DocNameCode.ToString()),
                CreateDateTimeSegment("133", jsonData.ETD, "203", jsonData.DocNameCode.ToString())
            };

            // NAD - Parties
            arrivalData.Parties = new SegmentData
            {
                SegmentID = "NAD",
                DataElements = new Element[]
                {
                    new DataElement { Name = "Party Qualifier", Value = "" },
                    new CompositeElement
                    {
                        DataElements = new DataElement[]
                        {
                            new DataElement { Name = "Party Id Identification", Value = jsonData.ShippingLineIdentification ?? "" },
                            new DataElement { Name = "Code List Qualifier", Value = "160" },
                            new DataElement { Name = "Code list responsible agency, coded", Value = "" }
                        }
                    },
                    new DataElement { Name = "Party name", Value = jsonData.ShippingLine ?? "" }
                }
            };

            return arrivalData;
        }

        public static List<Equipment> MapEquipmentFromJson(List<EquipmentData> equipmentDataList)
        {
            var equipments = new List<Equipment>();

            foreach (var jsonEquipment in equipmentDataList)
            {
                var equipment = new Equipment();

                // EQD - Equipment Details
                equipment.EquipmentDetails = new SegmentData
                {
                    SegmentID = "EQD",
                    DataElements = new Element[]
                    {
                        new DataElement { Name = "Equipment Qualifier", Value = "CN" },
                        new DataElement { Name = "Equipment Identification Number", Value = jsonEquipment.ContainerNumber ?? "" },
                        new CompositeElement
                        {
                            DataElements = new DataElement[]
                            {
                                new DataElement { Name = "Equipment size and type", Value = "" },
                                new DataElement { Name = "Code list identification code", Value = "102" },
                                new DataElement { Name = "Code list responsible agency code", Value = "5" }
                            }
                        },
                        new EmptyElement(),
                        new DataElement { Name = "Equipment status code", Value = jsonEquipment.OperationType.ToString() },
                        new DataElement { Name = "Full/Empty Indicator", Value = jsonEquipment.Condition.ToString() }
                    }
                };

                // RFF - References
                equipment.References = new List<SegmentData>
                {
                    new SegmentData
                    {
                        SegmentID = "RFF",
                        DataElements = new Element[]
                        {
                            new CompositeElement
                            {
                                DataElements = new DataElement[]
                                {
                                    new DataElement { Name = "Reference Qualifier", Value = "BM" },
                                    new DataElement { Name = "Reference Number", Value = jsonEquipment.BillOfLading ?? "" }
                                }
                            }
                        }
                    },
                    new SegmentData
                    {
                        SegmentID = "RFF",
                        DataElements = new Element[]
                        {
                            new CompositeElement
                            {
                                DataElements = new DataElement[]
                                {
                                    new DataElement { Name = "Reference Qualifier", Value = "ABT" },
                                    new DataElement { Name = "Reference identifier", Value = jsonEquipment.Dua ?? "" }
                                }
                            }
                        }
                    }
                };

                // DTM - Date
                equipment.Date = new SegmentData
                {
                    SegmentID = "DTM",
                    DataElements = new Element[]
                    {
                        new CompositeElement
                        {
                            DataElements = new DataElement[]
                            {
                                new DataElement { Name = "Date/Time/Period Qualifier", Value = "203" },
                                new DataElement { Name = "Date/Time/Period", Value = jsonEquipment.LoadingUnloadingDate ?? "" },
                                new DataElement { Name = "Date/Time/Period Format Qualifier", Value = "203" }
                            }
                        }
                    }
                };

                // LOC - Locations
                equipment.Locations = new List<SegmentData>
                {
                    new SegmentData
                    {
                        SegmentID = "LOC",
                        DataElements = new Element[]
                        {
                            new DataElement { Name = "Place/Location Qualifier", Value = "147" },
                            new DataElement { Name = "Location name code", Value = jsonEquipment.StowagePosition.ToString() }
                        }
                    },
                    new SegmentData
                    {
                        SegmentID = "LOC",
                        DataElements = new Element[]
                        {
                            new DataElement { Name = "Place/Location Qualifier", Value = "9" },
                            new DataElement { Name = "Location name code", Value = jsonEquipment.LoadingPort ?? "" }
                        }
                    },
                    new SegmentData
                    {
                        SegmentID = "LOC",
                        DataElements = new Element[]
                        {
                            new DataElement { Name = "Place/Location Qualifier", Value = "11" },
                            new DataElement { Name = "Location name code", Value = jsonEquipment.DischargePort ?? "" }
                        }
                    }
                };

                // MEA - Measurements
                equipment.Measurements = new SegmentData
                {
                    SegmentID = "MEA",
                    DataElements = new Element[]
                    {
                        new DataElement { Name = "Measurement Application Qualifier", Value = "AAE" },
                        new DataElement { Name = "Measurement Application Qualifier", Value = "VGM" },
                        new CompositeElement
                        {
                            DataElements = new DataElement[]
                            {
                                new DataElement { Name = "Measure Unit Qualifier", Value = "KGM" },
                                new DataElement { Name = "Measurement Value", Value = jsonEquipment.Weight.ToString() }
                            }
                        }
                    }
                };

                // TMP - Temperature (si aplica)
                if (!string.IsNullOrEmpty(jsonEquipment.Temperature))
                {
                    equipment.Temperature = new SegmentData
                    {
                        SegmentID = "TMP",
                        DataElements = new Element[]
                        {
                            new DataElement { Name = "Temperature Qualifier", Value = "2" },
                            new CompositeElement
                            {
                                DataElements = new DataElement[]
                                {
                                    new DataElement { Name = "Temperature value", Value = jsonEquipment.Temperature },
                                    new DataElement { Name = "Measure Unit Qualifier", Value = jsonEquipment.TemperatureUnit ?? "" }
                                }
                            }
                        }
                    };
                }

                // SEL - Seals (si aplica)
                if (jsonEquipment.Seals != null && jsonEquipment.Seals.Any())
                {
                    equipment.Seals = new List<SegmentData>();
                    foreach (var seal in jsonEquipment.Seals)
                    {
                        equipment.Seals.Add(new SegmentData
                        {
                            SegmentID = "SEL",
                            DataElements = new Element[]
                            {
                                new DataElement { Name = "Seal identifier", Value = seal.SealNumber ?? "" },
                                new CompositeElement
                                {
                                    DataElements = new DataElement[]
                                    {
                                        new DataElement { Name = "Sealing party name code", Value = seal.SealPartyNameCode ?? "" },
                                        new DataElement { Name = "SEAL TYPE CODE", Value = seal.SealType ?? "" }
                                    }
                                }
                            }
                        });
                    }
                }

                // DGS - Dangerous Goods (si aplica)
                if (jsonEquipment.HazardousCode != 0)
                {
                    equipment.DangerousGoods = new SegmentData
                    {
                        SegmentID = "DGS",
                        DataElements = new Element[]
                        {
                            new DataElement { Name = "Dangerous Goods Regulations", Value = "IMD" },
                            new CompositeElement
                            {
                                Name = "Hazard Code Identification",
                                DataElements = new DataElement[]
                                {
                                    new DataElement { Name = "Hazard Code Identification", Value = jsonEquipment.HazardousCode.ToString() }
                                }
                            },
                            new CompositeElement
                            {
                                Name = "United Nations Dangerous Goods (UNDG) identifier",
                                DataElements = new DataElement[]
                                {
                                    new DataElement { Name = "United Nations Dangerous Goods", Value = "" }
                                }
                            }
                        }
                    };

                    // FTX - Free Text (si aplica)
                    equipment.FreeText = new SegmentData
                    {
                        SegmentID = "FTX",
                        DataElements = new Element[]
                        {
                            new DataElement { Name = "Text Subject Qualifier", Value = "AAD" },
                            new EmptyElement(),
                            new EmptyElement(),
                            new CompositeElement
                            {
                                DataElements = new DataElement[]
                                {
                                    new DataElement { Name = "Free text", Value = "" }
                                }
                            }
                        }
                    };
                }

                // NAD - Parties
                equipment.Parties = new SegmentData
                {
                    SegmentID = "NAD",
                    DataElements = new Element[]
                    {
                        new DataElement { Name = "Party Qualifier", Value = "CF" },
                        new CompositeElement
                        {
                            DataElements = new DataElement[]
                            {
                                new DataElement { Name = "Party Id Identification", Value = jsonEquipment.CarrierIdentification ?? "" },
                                new DataElement { Name = "Code List Qualifier", Value = "" },
                                new DataElement { Name = "Code list responsible agency, coded", Value = "" }
                            }
                        }
                    }
                };

                equipments.Add(equipment);
            }

            return equipments;
        }

        private static string GetImoGisisCode(string portCode)
        {
            return portCode?.ToUpper() switch
            {
                "CRMOB" => "0002",
                "CRCLIO" => "0001",
                "CRPAP" => "0003",
                _ => null
            };
        }

        private static SegmentData CreateLocationSegment(string qualifier, string? code, string? docName, string? name = null)
        {
            var elements = new List<Element>
    {
        new DataElement { Name = "Place/Location Qualifier", Usage = RuleUsage.Mandatory,
                                    DataType = DataType.Alphanumeric, Value = qualifier }
    };

            // CASO ESPECIAL: Primer LOC (41/42)
            if (qualifier == "41" || qualifier == "42")
            {
                // Determinar código GISIS basado en el puerto de descarga
                string gisisCode = code?.StartsWith("CR") == true ?
                    (code == "CRMOB" ? "0002" :
                     code == "CRCLIO" ? "0001" :
                     "0000") :
                    null;

                if (gisisCode != null)
                {
                    elements.Add(new DataElement
                    {
                        Name = "First related location name code",
                        Usage = RuleUsage.Mandatory,
                        DataType = DataType.Alphanumeric,
                        Value = gisisCode
                    });
                }

                return new SegmentData
                {
                    SegmentID = "LOC",
                    DataElements = elements.ToArray()
                };
            }

            // CASO NORMAL: Otros LOC
            if (!string.IsNullOrEmpty(name))
            {
                // Elemento compuesto para LOC (C519)
                var locationComposite = new CompositeElement
                {
                    Name = "Place/Location Identification",
                    DataElements = new DataElement[]
                    {
                new DataElement { Name = "Location name code", Usage = RuleUsage.Mandatory,
                                    DataType = DataType.Alphanumeric, Value = code },
                new DataElement { Name = "Code list Identification", Value = "ZZZ" },
                new DataElement { Name = "Code list responsible agency, coded", Value = "98" },
                new DataElement { Name = "Location name", Value = name }
                    }
                };

                elements.Add(locationComposite);

                // Agregar código GISIS si es puerto CR
                if (code?.StartsWith("CR") == true)
                {
                    string gisisCode = code == "CRMOB" ? "0002" :  // APM Terminals
                                      code == "CRCLIO" ? "0001" :  // JAPDEVA
                                      "0000";  // Default para otros puertos CR

                    elements.Add(new DataElement
                    {
                        Name = "First related location name code",
                        Value = gisisCode
                    });
                }
            }
            else
            {
                elements.Add(new DataElement
                {
                    Name = "Location name code",
                    Usage = RuleUsage.Mandatory,
                    DataType = DataType.Alphanumeric,
                    Value = code
                });
            }

            return new SegmentData
            {
                SegmentID = "LOC",
                DataElements = elements.ToArray()
            };
        }

        private static SegmentData CreateDateTimeSegment(string qualifier, string? dateTime, string formatQualifier, string? docName)
        {
            return new SegmentData
            {
                SegmentID = "DTM",
                DataElements = new Element[]
                {
                    new CompositeElement
                    {
                        DataElements = new DataElement[]
                        {
                            new DataElement { Name = "Date/Time/Period Qualifier", Usage = RuleUsage.Mandatory,
                                    DataType = DataType.Alphanumeric, Value = qualifier },
                            new DataElement { Name = "Date/Time/Period", Value = dateTime },
                            new DataElement { Name = "Date/Time/Period Format Qualifier", Value = formatQualifier }
                        }
                    }
                }
            };
        }

        public string BuildMessage()
        {
            var ediMessage = new StringBuilder();
            int segmentCount = 0;
            int equipmentCount = 0;

            // Obtener el MessageRefNumber del UNH
            var messageRefNumber = _arrivalData?.MessageHeader?.DataElements?
                                  .OfType<DataElement>()
                                  .FirstOrDefault(de => de.Name == "Message Reference Number")?
                                  .Value ?? GenerateMessageRefNumber();

            // 1. Agregar segmentos de ArrivalData
            if (_arrivalData != null)
            {
                AppendSegment(ediMessage, _arrivalData.InterchangeHeader, ref segmentCount);
                AppendSegment(ediMessage, _arrivalData.MessageHeader, ref segmentCount);
                AppendSegment(ediMessage, _arrivalData.BeginningOfMessage, ref segmentCount);
                AppendSegment(ediMessage, _arrivalData.TransportInformation, ref segmentCount);
                AppendSegment(ediMessage, _arrivalData.Reference, ref segmentCount);

                foreach (var loc in _arrivalData.Locations)
                {
                    AppendSegment(ediMessage, loc, ref segmentCount);
                }

                foreach (var dtm in _arrivalData.DateTimes)
                {
                    AppendSegment(ediMessage, dtm, ref segmentCount);
                }

                AppendSegment(ediMessage, _arrivalData.Parties, ref segmentCount);
            }

            // 2. Agregar segmentos de Equipment
            foreach (var equipment in _equipments ?? new List<Equipment>())
            {
                equipmentCount++;
                AppendSegment(ediMessage, equipment.EquipmentDetails, ref segmentCount);

                foreach (var rff in equipment.References)
                {
                    AppendSegment(ediMessage, rff, ref segmentCount);
                }

                AppendSegment(ediMessage, equipment.Date, ref segmentCount);

                foreach (var loc in equipment.Locations)
                {
                    AppendSegment(ediMessage, loc, ref segmentCount);
                }

                AppendSegment(ediMessage, equipment.Measurements, ref segmentCount);

                if (equipment.Temperature != null)
                {
                    AppendSegment(ediMessage, equipment.Temperature, ref segmentCount);
                }

                if (equipment.Seals != null)
                {
                    foreach (var sel in equipment.Seals)
                    {
                        AppendSegment(ediMessage, sel, ref segmentCount);
                    }
                }

                if (equipment.DangerousGoods != null)
                {
                    AppendSegment(ediMessage, equipment.DangerousGoods, ref segmentCount);
                    AppendSegment(ediMessage, equipment.FreeText, ref segmentCount);
                }

                AppendSegment(ediMessage, equipment.Parties, ref segmentCount);
            }

            // 3. Agregar segmentos finales
            // CNT - Control Total
            var cntSegment = new SegmentData
            {
                SegmentID = "CNT",
                DataElements = new Element[]
                {
                    new CompositeElement
                    {
                        DataElements = new DataElement[]
                        {
                            new DataElement { Name = "Control total type code qualifier", Usage = RuleUsage.Mandatory,
                                    DataType = DataType.Alphanumeric, Value = "16" },
                            new DataElement { Name = "Control total value", Usage = RuleUsage.Mandatory,
                                    DataType = DataType.Alphanumeric, Value = equipmentCount.ToString() }
                        }
                    }
                }
            };
            AppendSegment(ediMessage, cntSegment, ref segmentCount);

            // UNT - Message Trailer
            var untSegment = new SegmentData
            {
                SegmentID = "UNT",
                DataElements = new Element[]
                {
                    new DataElement { Name = "Number of segments in the message", Usage = RuleUsage.Mandatory,
                                    DataType = DataType.Alphanumeric, Value = segmentCount.ToString("D6") },
                    new DataElement {
                        Name = "Message reference number", Usage = RuleUsage.Mandatory,
                                    DataType = DataType.Alphanumeric,
                        Value = _arrivalData?.MessageHeader?.DataElements?.FirstOrDefault()?.Value?.ToString() ?? ""
                    }
                }
            };
            AppendSegment(ediMessage, untSegment, ref segmentCount);

            // UNZ - Interchange Trailer
            var unzSegment = new SegmentData
            {
                SegmentID = "UNZ",
                DataElements = new Element[]
                {
                    new DataElement { Name = "Interchange Control Count", Usage = RuleUsage.Mandatory,
                                    DataType = DataType.Alphanumeric, Value = "1" },
                    new DataElement {
                        Name = "Interchange Control Reference", Usage = RuleUsage.Mandatory,
                                    DataType = DataType.Alphanumeric,
                        Value = _arrivalData?.InterchangeHeader?.DataElements?.LastOrDefault()?.Value?.ToString() ?? ""
                    }
                }
            };
            AppendSegment(ediMessage, unzSegment, ref segmentCount);

            return ediMessage.ToString();
        }

        private void AppendSegment(StringBuilder builder, SegmentData? segment, ref int segmentCount)
        {
            if (segment == null) return;

            builder.Append(segment.SegmentID);

            if (segment.DataElements != null)
            {
                foreach (var element in segment.DataElements)
                {
                    builder.Append('+');
                    if (element is CompositeElement composite)
                    {
                        builder.Append(string.Join(":", composite.DataElements?.Select(de => de.Value?.ToString() ?? "") ?? Array.Empty<string>()));
                    }
                    else if (!(element is EmptyElement))
                    {
                        builder.Append(element.Value?.ToString() ?? "");
                    }
                }
            }

            builder.AppendLine("'");
            segmentCount++;
        }

        public (bool isValid, List<string> errors) ValidateMandatoryFields()
        {
            var errors = new List<string>();

            // Validar solo presencia de datos clave (no segmentos)
            if (_arrivalData == null)
            {
                errors.Add("No se encontraron datos de llegada (ArrivalData).");
                return (false, errors);
            }

            // Ejemplo: Validar IMO del buque
            if (string.IsNullOrEmpty(_arrivalData.TransportInformation?.DataElements
                ?.FirstOrDefault(e => e.Name.Contains("IMO"))?.Value?.ToString()))
            {
                errors.Add("El IMO del buque es obligatorio.");
            }

            // Validar contenedores
            foreach (var eq in _equipments ?? Enumerable.Empty<Equipment>())
            {
                if (string.IsNullOrEmpty(eq.EquipmentDetails?.DataElements
                    ?.FirstOrDefault(e => e.Name == "Equipment Identification Number")?.Value?.ToString()))
                {
                    errors.Add($"El número de contenedor es obligatorio para todos los equipos.");
                    break;
                }
            }

            return (!errors.Any(), errors);
        }

        // Método auxiliar para validar segmentos
        private void ValidateSegment(SegmentData? segment, string segmentName, List<string> errors)
        {
            if (segment == null)
            {
                errors.Add($"Falta segmento obligatorio: {segmentName}");
                return;
            }

            var mandatoryElements = segment.DataElements?
                .Where(e => e.Usage == RuleUsage.Mandatory)
                .ToList();

            if (mandatoryElements == null) return;

            foreach (var element in mandatoryElements)
            {
                if (string.IsNullOrWhiteSpace(element.Value?.ToString()))
                {
                    errors.Add($"[{segmentName}] Campo obligatorio vacío: {element.Name}");
                }
                else if (element is DataElement dataElement && !IsValidDataType(dataElement.Value.ToString(), dataElement.DataType))
                {
                    if (element is DataElement dataElementt)
                    {
                        errors.Add($"[{segmentName}] Tipo de dato inválido para {dataElementt.Name}. Se esperaba {dataElementt.DataType}");
                    }
                }
            }
        }

        // Validación de tipos de dato
        private bool IsValidDataType(string value, DataType dataType)
        {
            return dataType switch
            {
                DataType.Numeric => value.All(char.IsDigit),
                DataType.Alphabetic => value.All(char.IsLetter),
                DataType.Alphanumeric => true,
                DataType.Decimal => decimal.TryParse(value, out _),
                _ => true
            };
        }

        public static void ValidateJsonBeforeMapping(RootData rootData)
        {
            var errors = new List<string>();

            // Validar ArrivalData
            if (rootData.ArrivalData == null)
            {
                errors.Add("El objeto ArrivalData es nulo en el JSON.");
            }
            else
            {
                if (string.IsNullOrEmpty(rootData.ArrivalData.ShipName))
                    errors.Add("ShipName es obligatorio en ArrivalData.");

                if (string.IsNullOrEmpty(rootData.ArrivalData.IMO.ToString()))
                    errors.Add("IMO del buque es obligatorio en ArrivalData.");

                if (rootData.ArrivalData.DocNameCode != 119 && rootData.ArrivalData.DocNameCode != 122)
                    errors.Add("DocNameCode debe ser 119 (descarga) o 122 (carga).");
            }

            // Validar Equipments
            if (rootData.Equipments == null || !rootData.Equipments.Any())
            {
                errors.Add("No hay equipos/contenedores en el JSON.");
            }
            else
            {
                foreach (var eq in rootData.Equipments.Select((e, i) => new { e, i }))
                {
                    if (string.IsNullOrEmpty(eq.e.ContainerNumber))
                        errors.Add($"ContainerNumber es obligatorio para el equipo #{eq.i + 1}.");

                    if (eq.e.Weight <= 0)
                        errors.Add($"Weight debe ser mayor que 0 para el equipo #{eq.i + 1}.");
                }
            }

            if (errors.Any())
            {
                throw new ArgumentException($"Errores en el JSON:\n{string.Join("\n", errors)}");
            }
        }

    }
}