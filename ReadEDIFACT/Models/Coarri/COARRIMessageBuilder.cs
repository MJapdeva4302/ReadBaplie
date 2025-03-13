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

            var unh = new UNH { MessageRefNumber = "244172", MessageTypeId = "COARRI", MessageTypeVersion = "D", MessageTypeRelease = "23A", ControllingAgency = "UN", AssociationAssigned = "ITG10" };
            coarriMessage.AppendLine(unh.ToEDIString());

            // Generación de cada contenedor
            int count = 0;
            string messageRef = "244172";
            foreach (var equipment in _equipments)
            {
                count++;

                // var unh = new UNH { MessageRef = messageRef, MessageType = "COARRI:D:23A:UN:ITG10" };
                // coarriMessage.AppendLine(unh.ToEDIString());

                // var bgm = new BGM { DocumentName = "270", DocumentNumber = "", MessageFunction = "9" };
                // coarriMessage.AppendLine(bgm.ToEDIString());

                // var tdt = new TDT { TransportStage = "20", VoyageNumber = _voyageNumber, ModeOfTransport = "1", TransportID = _transportID, VesselName = _vesselName };
                // coarriMessage.AppendLine(tdt.ToEDIString());

                // var dtm132 = new DTM { DateTimeQualifier = "132", DateTime = equipment.LoadingUnloadingDate, FormatQualifier = "203" };
                // coarriMessage.AppendLine(dtm132.ToEDIString());

                // var dtm133 = new DTM { DateTimeQualifier = "133", DateTime = equipment.LoadingUnloadingDate, FormatQualifier = "203" };
                // coarriMessage.AppendLine(dtm133.ToEDIString());

                // var nadCA = new NAD { PartyQualifier = "CA", PartyIdentification = $"{_transportID}:160:166" };
                // coarriMessage.AppendLine(nadCA.ToEDIString());

                // var nadCF = new NAD { PartyQualifier = "CF", PartyIdentification = $"{_transportID}:160:166" };
                // coarriMessage.AppendLine(nadCF.ToEDIString());

                // var eqd = new EQD { EquipmentQualifier = "CN", ContainerNumber = equipment.ContainerNumber, ContainerType = "45R1" }; // Ajusta el tipo de contenedor si es necesario
                // coarriMessage.AppendLine(eqd.ToEDIString());

                // var rffBN = new RFF { ReferenceQualifier = "BN", ReferenceNumber = equipment.TripIdentificationNumber };
                // coarriMessage.AppendLine(rffBN.ToEDIString());

                // var rffABT = new RFF { ReferenceQualifier = "ABT", ReferenceNumber = "" };
                // coarriMessage.AppendLine(rffABT.ToEDIString());

                // var tmd = new TMD { TransportMode = "3" };
                // coarriMessage.AppendLine(tmd.ToEDIString());

                // var dtm203 = new DTM { DateTimeQualifier = "203", DateTime = equipment.LoadingUnloadingDate, FormatQualifier = "203" };
                // coarriMessage.AppendLine(dtm203.ToEDIString());

                // var loc147 = new LOC { LocationQualifier = "147", LocationCode = equipment.DischargePort };
                // coarriMessage.AppendLine(loc147.ToEDIString());

                // var loc11 = new LOC { LocationQualifier = "11", LocationCode = equipment.LoadingPort };
                // coarriMessage.AppendLine(loc11.ToEDIString());

                // var loc9 = new LOC { LocationQualifier = "9", LocationCode = _location };
                // coarriMessage.AppendLine(loc9.ToEDIString());

                // var meaVGM = new MEA { MeasurementQualifier = "AAE", Weight = equipment.VerifyGrossMass };
                // coarriMessage.AppendLine(meaVGM.ToEDIString());

                // var meaG = new MEA { MeasurementQualifier = "AAE", Weight = equipment.Weight };
                // coarriMessage.AppendLine(meaG.ToEDIString());

                // var tmp = new TMP { TemperatureQualifier = "2", Temperature = $"{equipment.Temperature}:{equipment.TemperatureUnit}" };
                // coarriMessage.AppendLine(tmp.ToEDIString());

                // foreach (var seal in equipment.Seals)
                // {
                //     var sel = new SEL { SealNumber = seal.SealNumber };
                //     coarriMessage.AppendLine(sel.ToEDIString());
                // }

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