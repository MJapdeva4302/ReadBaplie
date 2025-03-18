using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;

namespace ReadEDIFACT.Models.Coarri
{
    public class COARRIMessageBuilder
    {
        private List<Equipment> _equipments;

        public COARRIMessageBuilder(List<Equipment> equipments)
        {
            _equipments = equipments;
            
        }

        public string BuildMessage()
        {
            StringBuilder coarriMessage = new StringBuilder();

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

            var bgm = new BGM { DocumentName = "", DocumentNumber = "", MessageFunction = "9" };
            coarriMessage.AppendLine(bgm.ToEDIString());

            var tdt = new TDT { TransportStage = "", TransportMeansJourney = "", TransportModeName = "", CarrierIdentifier = "", CodeListIdentification = "", CodeListAgency = "", CarrierName = "", TransportMeanIdentification = "", CodeListIdentificationTwo = "", CodeListResponsibleAgency = "", TransportIDName = "" };
            coarriMessage.AppendLine(tdt.ToEDIString());

            var dtmETA = new DTM { DateOrTimeQualifier = "", DateOrTime = "", DateOrTimeFormatQualifier = ""};
            coarriMessage.AppendLine(dtmETA.ReturnFormat(null, "2021-09-01"));

            var dtmETD = new DTM { DateOrTimeQualifier = "", DateOrTime = "", DateOrTimeFormatQualifier = ""};
            coarriMessage.AppendLine(dtmETD.ReturnFormat(null, "2021-09-01"));

            var rff = new RFF { ReferenceQualifier = "", ReferenceIdentifier = "" };
            coarriMessage.AppendLine(rff.ToEDIString());


            var loc1 = new LOC { LocationQualifier = "", LocationCode = "", BGM = bgm };
            coarriMessage.AppendLine(loc1.ToEDIString());
            var loc2 = new LOC { LocationQualifier = "", LocationCode = "", BGM = bgm };
            coarriMessage.AppendLine(loc2.ToCustomEDI());
            var loc3 = new LOC { LocationQualifier = "", LocationCode = "", BGM = bgm };
            coarriMessage.AppendLine(loc3.ToCustomEDI());
            var loc4 = new LOC { LocationQualifier = "", LocationCode = "", BGM = bgm };
            coarriMessage.AppendLine(loc4.ToCustomEDI());

            var dtm1 = new DTM { DateOrTimeQualifier = "", DateOrTime = "", DateOrTimeFormatQualifier = "", BGM = bgm };
            coarriMessage.AppendLine(dtm1.ToCustomEDI());

            var nad = new NAD { PartyQualifier = "", PartyIdentifier = "", CodeListIdentification = "", CodeListResponsibleAgency = "" , PartyName = ""};
            coarriMessage.AppendLine(nad.ToEDIString());

            // Generación de cada contenedor
            int count = 0;
            string messageRef = "244172";
            foreach (var equipment in _equipments)
            {
                count++;
                var eqd = new EQD { EquipmentQualifier = "CN", ContainerNumber = equipment.EquipmentDetails.ContainerNumber, EquipmentSizeAndType = equipment.EquipmentDetails.EquipmentSizeAndType, CodeListIdentification = "", CodeListResponsibleAgency = "", EquipmentStatusCode = "", FullEmptyIndicator = equipment.EquipmentDetails.FullEmptyIndicator };
                coarriMessage.AppendLine(eqd.ToEDIString());

                var rffBN = new RFF { ReferenceQualifier = "BN", ReferenceIdentifier = equipment.Reference.ReferenceIdentifier, EQD = eqd };
                coarriMessage.AppendLine(rffBN.ToCustomEDI());

                var dtm203 = new DTM { DateOrTimeQualifier = "203", DateOrTime = equipment.Date.DateOrTime, DateOrTimeFormatQualifier = "203" };
                coarriMessage.AppendLine(dtm203.DateOperation(equipment.Date.DateOrTime));

                var loc147 = new LOC { LocationQualifier = "147", LocationCode = equipment.Location.LocationQualifier};
                coarriMessage.AppendLine(loc147.ToCustomEDI());

                var loc9 = new LOC { LocationQualifier = "9", LocationCode = equipment.Location.LocationCode};
                coarriMessage.AppendLine(loc9.ToCustomEDI());

                var loc11 = new LOC { LocationQualifier = "11", LocationCode = equipment.Location.LocationCode};
                coarriMessage.AppendLine(loc11.Location("11", loc11.LocationCode, "", ""));

                var meaVGM = new MEA { MeasurementQualifier = "AAE", MeasurementAttribute = equipment.Measurements.MeasurementAttribute, MeasurementValue = equipment.Measurements.MeasurementValue };
                coarriMessage.AppendLine(meaVGM.ToEDIString());

                var tmp = new TMP { TemperatureQualifier = "2", TemperatureValue = equipment.Temperature.TemperatureValue, TemperatureUnit = equipment.Temperature.TemperatureUnit };
                coarriMessage.AppendLine(tmp.ToEDIString());

                foreach (var seal in equipment.Seals)
                {
                    var sel = new SEL { SealNumber = seal.SealNumber, SealType = seal.SealType };
                    coarriMessage.AppendLine(sel.ToEDIString());
                }

                if(equipment.DangerousGoods != null)
                {
                    
                        var dgs = new DGS { DangerousGoodsCode = equipment.DangerousGoods.DangerousGoodsCode, HazardIdentificationCode = equipment.DangerousGoods.HazardIdentificationCode, DangerousGoodsClassificationCode = equipment.DangerousGoods.DangerousGoodsClassificationCode };
                        coarriMessage.AppendLine(dgs.ToCustomEDI());
                        var ftx = new FTX { TextSubjectCode = "AAD", TextValue = equipment.FreeText.TextValue };
                        coarriMessage.AppendLine(ftx.ToEDIString());
                }
                
                    var unt = new UNT { SegmentCount = "000023", MessageRef = messageRef };
                    coarriMessage.AppendLine(unt.ToEDIString());
                

                // var cnt = new CNT { ControlTotalQualifier = "16", ControlTotalValue = "1" };
                // coarriMessage.AppendLine(cnt.ToEDIString());

                // var unt = new UNT { SegmentCount = "000023", MessageRef = messageRef };
                // coarriMessage.AppendLine(unt.ToEDIString());

                // messageRef = (int.Parse(messageRef) + 1).ToString(); // Incrementar referencia del mensaje
            }

            // Cierre del intercambio
            // var unz = new UNZ { InterchangeControlCount = _equipments.Count.ToString(), InterchangeRef = _interchangeRef };
            // coarriMessage.AppendLine(unz.ToEDIString());

            return coarriMessage.ToString();
        }
    }
}