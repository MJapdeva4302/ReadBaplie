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

        public COARRIMessageBuilder(List<Equipment> equipments)
        {
            _equipments = equipments;

        }

        // Método para cargar el JSON desde un archivo
        public static RootData LoadJson(string filePath)
        {
            string jsonString = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<RootData>(jsonString);
        }

        // Método para mapear el JSON a la estructura de la clase Equipment
        // public static List<Equipment> MapFromJson(RootData jsonData)
        // {
        //     var equipments = new List<Equipment>();

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
        //                 EquipmentStatusCode = "",
        //                 FullEmptyIndicator = jsonEquipment.Condition.ToString(),
        //             },
        //             Reference = new RFF
        //             {
        //                 ReferenceIdentifier = jsonEquipment.TripIdentificationNumber,
        //                 ReferenceQualifier = "BN"
        //             },
        //             Date = new DTM
        //             {
        //                 DateOrTime = jsonEquipment.LoadingUnloadingDate,
        //                 DateOrTimeQualifier = "203",
        //                 DateOrTimeFormatQualifier = "203"
        //             },
        //             Location = new LOC
        //             {
        //                 LocationCode = jsonEquipment.DischargePort,
        //                 LocationQualifier = "9" 
        //             },
        //             Measurements = new MEA
        //             {
        //                 MeasurementQualifier = "AAE",
        //                 MeasurementAttribute = "VGM",
        //                 MeasurementValue = jsonEquipment.VerifyGrossMass,
        //                 WeightUnitCode = "KGM"
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
        //                 SealType = seal.SealType
        //             }).ToList(),
        //             DangerousGoods = jsonEquipment.HazardousCode != 0 ? new DGS
        //             {
        //                 DangerousGoodsCode = jsonEquipment.HazardousCode.ToString(),
        //                 HazardIdentificationCode = "", 
        //                 DangerousGoodsClassificationCode = "" 
        //             } : null,
        //             FreeText = new FTX
        //             {
        //                 TextSubjectCode = "AAD",
        //                 TextValue = "" 
        //             },
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
            int segmentCount = 0; // Contador de segmentos
            // Encabezado del intercambio
            var unb = new UNB
            {
                SyntaxIdentifier = "UNOA",
                SyntaxVersion = "2",
                SenderIdentification = "",
                ReceiverIdentification = "",
                Date = DateTime.Now.ToString("yyMMdd"),
                Time = DateTime.Now.ToString("HHmm"),
                InterchangeRef = ""
            };
            coarriMessage.AppendLine(unb.ToEDIString());

            var unh = new UNH { MessageRefNumber = "", MessageTypeId = "COARRI", MessageTypeVersion = "D", MessageTypeRelease = "23A", ControllingAgency = "UN", AssociationAssigned = "ITG10" };
            coarriMessage.AppendLine(unh.ToEDIString());
            segmentCount++;

            var bgm = new BGM { DocumentName = "", DocumentNumber = "", MessageFunction = "9" };
            coarriMessage.AppendLine(bgm.ToEDIString());
            segmentCount++;

            var tdt = new TDT { TransportStage = "", TransportMeansJourney = "", TransportModeName = "", CarrierIdentifier = "", CodeListIdentification = "", CodeListAgency = "", CarrierName = "", TransportMeanIdentification = "", CodeListIdentificationTwo = "", CodeListResponsibleAgency = "", TransportIDName = "" };
            coarriMessage.AppendLine(tdt.ToEDIString());
            segmentCount++;

            var dtmETA = new DTM { DateOrTimeQualifier = "", DateOrTime = "", DateOrTimeFormatQualifier = "" };
            coarriMessage.AppendLine(dtmETA.ReturnFormat(null, "2021-09-01"));
            segmentCount++;

            var dtmETD = new DTM { DateOrTimeQualifier = "", DateOrTime = "", DateOrTimeFormatQualifier = "" };
            coarriMessage.AppendLine(dtmETD.ReturnFormat(null, "2021-09-01"));
            segmentCount++;

            var rff = new RFF { ReferenceQualifier = "", ReferenceIdentifier = "" };
            coarriMessage.AppendLine(rff.ToEDIString());
            segmentCount++;


            var loc1 = new LOC { LocationQualifier = "", LocationCode = "", BGM = bgm };
            coarriMessage.AppendLine(loc1.ToEDIString());
            segmentCount++;
            var loc2 = new LOC { LocationQualifier = "", LocationCode = "", BGM = bgm };
            coarriMessage.AppendLine(loc2.ToCustomEDI());
            segmentCount++;
            var loc3 = new LOC { LocationQualifier = "", LocationCode = "", BGM = bgm };
            coarriMessage.AppendLine(loc3.ToCustomEDI());
            segmentCount++;
            var loc4 = new LOC { LocationQualifier = "", LocationCode = "", BGM = bgm };
            coarriMessage.AppendLine(loc4.ToCustomEDI());
            segmentCount++;

            var dtm1 = new DTM { DateOrTimeQualifier = "", DateOrTime = "", DateOrTimeFormatQualifier = "", BGM = bgm };
            coarriMessage.AppendLine(dtm1.ToCustomEDI());
            segmentCount++;

            var nad = new NAD { PartyQualifier = "", PartyIdentifier = "", CodeListIdentification = "", CodeListResponsibleAgency = "", PartyName = "" };
            coarriMessage.AppendLine(nad.ToEDIString());
            segmentCount++;

            // Generación de cada contenedor
            int count = 0;
            foreach (var equipment in _equipments)
            {
                count++;
                var eqd = new EQD { EquipmentQualifier = "CN", ContainerNumber = equipment.EquipmentDetails.ContainerNumber, EquipmentSizeAndType = equipment.EquipmentDetails.EquipmentSizeAndType, CodeListIdentification = "", CodeListResponsibleAgency = "", EquipmentStatusCode = "", FullEmptyIndicator = equipment.EquipmentDetails.FullEmptyIndicator };
                coarriMessage.AppendLine(eqd.ToEDIString());
                segmentCount++;

                var rffBN = new RFF { ReferenceQualifier = "BN", ReferenceIdentifier = equipment.Reference.ReferenceIdentifier, EQD = eqd };
                coarriMessage.AppendLine(rffBN.ToCustomEDI());
                segmentCount++;

                var dtm203 = new DTM { DateOrTimeQualifier = "203", DateOrTime = equipment.Date.DateOrTime, DateOrTimeFormatQualifier = "203" };
                coarriMessage.AppendLine(dtm203.DateOperation(equipment.Date.DateOrTime));
                segmentCount++;

                var loc147 = new LOC { LocationQualifier = "147", LocationCode = equipment.Location.LocationQualifier };
                coarriMessage.AppendLine(loc147.ToCustomEDI());
                segmentCount++;

                var loc9 = new LOC { LocationQualifier = "9", LocationCode = equipment.Location.LocationCode };
                coarriMessage.AppendLine(loc9.ToCustomEDI());
                segmentCount++;

                var loc11 = new LOC { LocationQualifier = "11", LocationCode = equipment.Location.LocationCode };
                coarriMessage.AppendLine(loc11.Location("11", loc11.LocationCode, "", ""));
                segmentCount++;

                var meaVGM = new MEA { MeasurementQualifier = "AAE", MeasurementAttribute = equipment.Measurements.MeasurementAttribute, MeasurementValue = equipment.Measurements.MeasurementValue };
                coarriMessage.AppendLine(meaVGM.ToEDIString());
                segmentCount++;

                var tmp = new TMP { TemperatureQualifier = "2", TemperatureValue = equipment.Temperature.TemperatureValue, TemperatureUnit = equipment.Temperature.TemperatureUnit };
                coarriMessage.AppendLine(tmp.ToEDIString());
                segmentCount++;

                foreach (var seal in equipment.Seals)
                {
                    var sel = new SEL { SealNumber = seal.SealNumber, SealType = seal.SealType };
                    coarriMessage.AppendLine(sel.ToEDIString());
                    segmentCount++;
                }

                if (equipment.DangerousGoods != null)
                {

                    var dgs = new DGS { DangerousGoodsCode = equipment.DangerousGoods.DangerousGoodsCode, HazardIdentificationCode = equipment.DangerousGoods.HazardIdentificationCode, DangerousGoodsClassificationCode = equipment.DangerousGoods.DangerousGoodsClassificationCode };
                    coarriMessage.AppendLine(dgs.ToCustomEDI());
                    segmentCount++;

                    var ftx = new FTX { TextSubjectCode = "AAD", TextValue = equipment.FreeText.TextValue };
                    coarriMessage.AppendLine(ftx.ToEDIString());
                    segmentCount++;
                }

                var nadCF = new NAD { PartyQualifier = "CF", PartyIdentifier = equipment.Parties.PartyIdentifier, CodeListIdentification = equipment.Parties.CodeListIdentification, CodeListResponsibleAgency = equipment.Parties.CodeListResponsibleAgency };
                coarriMessage.AppendLine(nadCF.ToEDIString());
                segmentCount++;


                // messageRef = (int.Parse(messageRef) + 1).ToString(); // Incrementar referencia del mensaje
            }
                var cnt = new CNT { ControlTotalQualifier = "16", ControlTotalValue = "1" };
                coarriMessage.AppendLine(cnt.ToEDIString());
                segmentCount++;

                var unt = new UNT { SegmentCount = segmentCount.ToString("D6"), MessageRef = unh.MessageRefNumber };
                coarriMessage.AppendLine(unt.ToEDIString());

            // Cierre del intercambio
            var unz = new UNZ { InterchangeControlCount = _equipments.Count.ToString(), MessageRef = unb.InterchangeRef };
            coarriMessage.AppendLine(unz.ToEDIString());

            return coarriMessage.ToString();
        }
    }
}