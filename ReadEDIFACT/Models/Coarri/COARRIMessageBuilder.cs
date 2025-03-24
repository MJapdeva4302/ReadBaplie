using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using Newtonsoft.Json;

namespace ReadEDIFACT.Models.Coarri
{
    public class COARRIMessageBuilder
    {
        private List<Equipment> _equipments;
        private ArrivalData _arrivalData;

        public COARRIMessageBuilder(ArrivalData arrivalData, List<Equipment> equipments)
        {
            _arrivalData = arrivalData;
            _equipments = equipments;
        }

        // Método para cargar el JSON desde un archivo
        public static RootData LoadJson(string filePath)
        {
            string jsonString = File.ReadAllText(filePath);
            var result = JsonConvert.DeserializeObject<RootData>(jsonString);
            if (result == null)
            {
                throw new InvalidOperationException("Failed to deserialize JSON to RootData.");
            }
            return result;
        }

        // Método para mapear el JSON a la estructura de la clase ArrivalData
        // public static ArrivalData MapArrivalDataFromJson(RootData jsonData)
        // {
        //      var arrivalData = jsonData.ArrivalData;

        //     var result = new ArrivalData
        //     {
        //         // Interchange Header (UNB) - datos básicos del intercambio
        //         InterchangeHeader = new UNB
        //         {
        //             SyntaxIdentifier = "UNOA",
        //             SyntaxVersion = "2",
        //             SenderIdentification = "", // Deberías poner aquí tu identificación
        //             ReceiverIdentification = "", // Identificación del receptor
        //             Date = DateTime.UtcNow.ToString("yyMMdd"),
        //             Time = DateTime.UtcNow.ToString("HHmm"),
        //             InterchangeRef = ""
        //         },

        //         // Message Header (UNH)
        //         MessageHeader = new UNH
        //         {
        //             MessageRefNumber = "", // Número de referencia del mensaje
        //             MessageTypeId = "COARRI",
        //             MessageTypeVersion = "D",
        //             MessageTypeRelease = "23A",
        //             ControllingAgency = "UN",
        //             AssociationAssigned = "ITG10"
        //         },

        //         // Beginning of Message (BGM)
        //         BeginningOfMessage = new BGM
        //         {
        //             DocumentName = arrivalData.DocNameCode, // Código del documento
        //             DocumentNumber = arrivalData.ArrivalNumber, // Número de arribo
        //             MessageFunction = "9" // 9 = original
        //         },

        //         // Transport Information (TDT)
        //         TransportInformation = new TDT
        //         {
        //             TransportStage = "20", // 20 = transporte principal
        //             TransportMeansJourney = "133",
        //             TransportModeName = "1", // 1 = marítimo
        //             CarrierIdentifier = "",
        //             CodeListIdentification = "172",
        //             CarrierName = arrivalData.ShipName,
        //             TransportMeanIdentification = arrivalData.IMO,
        //             TransportIDName = arrivalData.ShipName
        //         },

        //         // Reference (RFF) - CallSign
        //         Reference = new RFF
        //         {
        //             ReferenceQualifier = "VM", // Código para CallSign
        //             ReferenceIdentifier = arrivalData.CallSign ?? ""
        //         },

        //         // Locations (LOC)
        //         Location1 = new LOC
        //         {
        //             LocationQualifier = arrivalData.LocFunctionCode, // 41 = función del lugar (carga/descarga)
        //             LocationCode = ""
        //         },

        //         Location2 = new LOC
        //         {
        //             LocationQualifier = "9",
        //             LocationCode = arrivalData.LoadingPortCode,
        //             LocationName = arrivalData.LoadingPortName
        //         },

        //         Location3 = new LOC
        //         {
        //             LocationQualifier = "94",
        //             LocationCode = arrivalData.OriginPort,
        //             LocationName = arrivalData.OriginPortName
        //         },

        //         Location4 = new LOC
        //         {
        //             LocationQualifier = "11",
        //             LocationCode = arrivalData.DischargePortCode,
        //             LocationName = arrivalData.DischargePortName
        //         },

        //         Location5 = new LOC
        //         {
        //             LocationQualifier = "61",
        //             LocationCode = arrivalData.DestinationPort,
        //             LocationName = arrivalData.DestinationPortName
        //         },

        //         // Date/Time (DTM)
        //         Date1 = new DTM
        //         {
        //             DateOrTimeQualifier = "203",
        //             DateOrTime = arrivalData.ETA,
        //             DateOrTimeFormatQualifier = "203"
        //         },

        //         Date2 = new DTM
        //         {
        //             DateOrTimeQualifier = "203",
        //             DateOrTime = arrivalData.ETD,
        //             DateOrTimeFormatQualifier = "203"
        //         },

        //         // Parties (NAD) - Shipping Line
        //         Parties = new NAD
        //         {
        //             PartyQualifier = "", // Código para línea naviera
        //             PartyIdentifier = "",
        //             CodeListIdentification = "",
        //             CodeListResponsibleAgency = "",
        //             PartyName = ""
        //         }
        //     };

        //     return result;
        // }

        // // Método para mapear el JSON a la estructura de la clase Equipment
        // public List<Equipment> MapEquipmentFromJson(RootData jsonData)
        // {
        //     _equipments = new List<Equipment>();

        //     foreach (var jsonEquipment in jsonData.Equipments)
        //     {
        //         var equipment = new Equipment
        //         {
        //             EquipmentDetails = new EQD
        //             {
        //                 EquipmentQualifier = "CN",
        //                 ContainerNumber = jsonEquipment.ContainerNumber,
        //                 EquipmentSizeAndType = "",
        //                 CodeListIdentification = "",
        //                 CodeListResponsibleAgency = "",
        //                 EquipmentStatusCode = jsonEquipment.OperationType.ToString(),
        //                 FullEmptyIndicator = jsonEquipment.Condition.ToString(),
        //             },
        //             Reference1 = new RFF
        //             {
        //                 ReferenceQualifier = "BM",
        //                 ReferenceIdentifier = jsonEquipment.BillOfLading
        //             },

        //             Reference2 = new RFF
        //             {
        //                 ReferenceQualifier = "ABT",
        //                 ReferenceIdentifier = jsonEquipment.TripIdentificationNumber
        //             },

        //             Date = new DTM
        //             {
        //                 DateOrTimeQualifier = "203",
        //                 DateOrTime = jsonEquipment.LoadingUnloadingDate,
        //                 DateOrTimeFormatQualifier = "203"
        //             },

        //             Location1 = new LOC
        //             {
        //                 LocationQualifier = "147",
        //                 LocationCode = jsonEquipment.StowagePosition,
        //             },
        //             Location2 = new LOC
        //             {
        //                 LocationQualifier = "9",
        //                 LocationCode = jsonEquipment.LoadingPort
        //             },
        //             Location3 = new LOC
        //             {
        //                 LocationQualifier = "11",
        //                 LocationCode = jsonEquipment.DischargePort
        //             },

        //             Measurements = new MEA
        //             {
        //                 MeasurementQualifier = "AAE",
        //                 MeasurementAttribute = "VGM",
        //                 WeightUnitCode = "KGM",
        //                 MeasurementValue = jsonEquipment.Weight
        //             },

        //             Temperature = new TMP
        //             {
        //                 TemperatureQualifier = "2",
        //                 TemperatureValue = jsonEquipment.Temperature,
        //                 TemperatureUnit = jsonEquipment.TemperatureUnit
        //             },

        //             Seals = jsonEquipment.Seals.Select(seal => new SEL
        //             {
        //                 SealNumber = seal.SealNumber,
        //                 SealPartyNameCode = seal.SealPartyNameCode,
        //                 SealType = seal.SealType
        //             }).ToList(),

        //             DangerousGoods = jsonEquipment.HazardousCode != 0 ? new DGS
        //             {
        //                 DangerousGoodsCode = jsonEquipment.HazardousCode.ToString(),
        //                 HazardIdentificationCode = "",
        //                 DangerousGoodsClassificationCode = ""
        //             } : null,

        //             FreeText = jsonEquipment.HazardousCode != 0 ? new FTX
        //             {
        //                 TextSubjectCode = "AAD",
        //                 TextValue = ""
        //             } : null,

        //             Parties = new NAD
        //             {
        //                 PartyQualifier = "CF",
        //                 PartyIdentifier = jsonEquipment.CarrierIdentification,
        //                 CodeListIdentification = "",
        //                 CodeListResponsibleAgency = ""
        //             }
        //         };

        //         equipments.Add(equipment);
        //     }

        //     return equipments;
        // }

        public string BuildMessage()
        {

            StringBuilder coarriMessage = new StringBuilder();
            int segmentCount = 0;

            // Encabezado del intercambio (UNB) - Usando datos del JSON
            var unb = new UNB
            {
                SyntaxIdentifier = _arrivalData.InterchangeHeader.SyntaxIdentifier,
                SyntaxVersion = _arrivalData.InterchangeHeader.SyntaxVersion,
                SenderIdentification = _arrivalData.InterchangeHeader.SenderIdentification,
                ReceiverIdentification = _arrivalData.InterchangeHeader.ReceiverIdentification,
                Date = _arrivalData.InterchangeHeader.Date,
                Time = _arrivalData.InterchangeHeader.Time,
                InterchangeRef = _arrivalData.InterchangeHeader.InterchangeRef
            };
            coarriMessage.AppendLine(unb.ToEDIString());

            // Message Header (UNH) - Usando datos del JSON
            coarriMessage.AppendLine(new UNH
            {
                MessageRefNumber = _arrivalData.MessageHeader.MessageRefNumber,
                MessageTypeId = _arrivalData.MessageHeader.MessageTypeId,
                MessageTypeVersion = _arrivalData.MessageHeader.MessageTypeVersion,
                MessageTypeRelease = _arrivalData.MessageHeader.MessageTypeRelease,
                ControllingAgency = _arrivalData.MessageHeader.ControllingAgency,
                AssociationAssigned = _arrivalData.MessageHeader.AssociationAssigned
            }.ToEDIString());
           
            segmentCount++;

            // Beginning of Message (BGM)
            var bgm = new BGM
            {
                DocumentName = _arrivalData.BeginningOfMessage.DocumentName,
                DocumentNumber = _arrivalData.BeginningOfMessage.DocumentNumber,
                MessageFunction = _arrivalData.BeginningOfMessage.MessageFunction
            };
            coarriMessage.AppendLine(bgm.ToEDIString());
            segmentCount++;

            // Transport Information (TDT)
            var tdt = new TDT
            {
                TransportStage = _arrivalData.TransportInformation.TransportStage,
                TransportMeansJourney = _arrivalData.TransportInformation.TransportMeansJourney,
                TransportModeName = _arrivalData.TransportInformation.TransportModeName,
                CarrierIdentifier = _arrivalData.TransportInformation.CarrierIdentifier,
                CodeListIdentification = _arrivalData.TransportInformation.CodeListIdentification,
                CarrierName = _arrivalData.TransportInformation.CarrierName,
                TransportMeanIdentification = _arrivalData.TransportInformation.TransportMeanIdentification,
                TransportIDName = _arrivalData.TransportInformation.TransportIDName
            };
            coarriMessage.AppendLine(tdt.ToEDIString());
            segmentCount++;

            // Reference (RFF) - CallSign
            var rff = new RFF
            {
                ReferenceQualifier = _arrivalData.Reference.ReferenceQualifier,
                ReferenceIdentifier = _arrivalData.Reference.ReferenceIdentifier
            };
            coarriMessage.AppendLine(rff.ToEDIString());
            segmentCount++;

            // Locations (LOC)
            coarriMessage.AppendLine(_arrivalData.Location1.ToEDIString());
            segmentCount++;
            coarriMessage.AppendLine(_arrivalData.Location2.ToCustomEDI());
            segmentCount++;
            coarriMessage.AppendLine(_arrivalData.Location3.ToCustomEDI());
            segmentCount++;
            coarriMessage.AppendLine(_arrivalData.Location4.ToCustomEDI());
            segmentCount++;
            coarriMessage.AppendLine(_arrivalData.Location5.ToCustomEDI());
            segmentCount++;

            // Date/Time (DTM)
            coarriMessage.AppendLine(_arrivalData.Date1.ToCustomEDI());
            segmentCount++;
            coarriMessage.AppendLine(_arrivalData.Date2.ToCustomEDI());
            segmentCount++;

            // Parties (NAD) - Shipping Line
            var nad = new NAD
            {
                PartyQualifier = _arrivalData.Parties.PartyQualifier,
                PartyIdentifier = _arrivalData.Parties.PartyIdentifier,
                CodeListIdentification = _arrivalData.Parties.CodeListIdentification,
                CodeListResponsibleAgency = _arrivalData.Parties.CodeListResponsibleAgency,
                PartyName = _arrivalData.Parties.PartyName
            };
            coarriMessage.AppendLine(nad.ToEDIString());
            segmentCount++;

            // Generación de cada contenedor (Equipment)
            foreach (var equipment in _equipments)
            {
                // Equipment Details (EQD)
                coarriMessage.AppendLine(equipment.EquipmentDetails.ToEDIString());
                segmentCount++;

                // References (RFF)
                coarriMessage.AppendLine(equipment.Reference1.ToCustomEDI());
                segmentCount++;
                coarriMessage.AppendLine(equipment.Reference2.ToCustomEDI());
                segmentCount++;

                // Date (DTM)
                coarriMessage.AppendLine(equipment.Date.DateOperation(equipment.Date.DateOrTime));
                segmentCount++;

                // Locations (LOC)
                coarriMessage.AppendLine(equipment.Location1.Location("147", equipment.Location1.LocationCode, "", ""));
                segmentCount++;
                coarriMessage.AppendLine(equipment.Location2.Location("9", equipment.Location2.LocationCode, "", ""));
                segmentCount++;
                coarriMessage.AppendLine(equipment.Location3.Location("11", equipment.Location3.LocationCode, "", ""));
                segmentCount++;

                // Measurements (MEA)
                coarriMessage.AppendLine(equipment.Measurements.ToEDIString());
                segmentCount++;

                // Temperature (TMP)
                if (equipment.Temperature != null)
                {
                    coarriMessage.AppendLine(equipment.Temperature.ToEDIString());
                    segmentCount++;
                }

                // Seals (SEL)
                foreach (var seal in equipment.Seals)
                {
                    coarriMessage.AppendLine(seal.ToEDIString());
                    segmentCount++;
                }

                // Dangerous Goods (DGS) - Solo si existe
                if (equipment.DangerousGoods != null)
                {
                    coarriMessage.AppendLine(equipment.DangerousGoods.ToCustomEDI());
                    segmentCount++;

                    // Free Text (FTX) - Relacionado con Dangerous Goods
                    if (equipment.FreeText != null)
                    {
                        coarriMessage.AppendLine(equipment.FreeText.ToEDIString());
                        segmentCount++;
                    }
                }

                // Parties (NAD)
                coarriMessage.AppendLine(equipment.Parties.ToEDIString());
                segmentCount++;
            }

            // Control Total (CNT)
            var cnt = new CNT { ControlTotalQualifier = "16", ControlTotalValue = "1" };
            coarriMessage.AppendLine(cnt.ToEDIString());
            segmentCount++;

            // Message Trailer (UNT)
            var unt = new UNT { SegmentCount = segmentCount.ToString("D6"), MessageRef = _arrivalData.MessageHeader.MessageRefNumber };
            coarriMessage.AppendLine(unt.ToEDIString());

            // Interchange Trailer (UNZ)
            var unz = new UNZ { InterchangeControlCount = "1", MessageRef = _arrivalData.InterchangeHeader.InterchangeRef };
            coarriMessage.AppendLine(unz.ToEDIString());

            return coarriMessage.ToString();
        }
    }
}