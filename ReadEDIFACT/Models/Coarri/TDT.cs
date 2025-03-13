using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReadEDIFACT.Models.Coarri
{
    public class TDT: EDISegment
    {
        // 20
        public string TransportStage { get; set; }
        // Means of transport journey identifier = 133
        public string TransportMeansJourney { get; set; }
        // Transport mode name code = 1
        public string TransportModeName { get; set; }
        // ACA VA UN ESPACIO VACIO

        // Carrier identification = CHQ
        public string CarrierIdentifier { get; set; }
        // CODE LIST IDENTIFICATION CODE = 172
        public string CodeListIdentification { get; set; }
        // Code list responsible agency code = ZZZ, 166, 87, 20
        public string CodeListAgency { get; set; }
        // Carrier name = CHIQUITA
        public string CarrierName { get; set; }
        // Transport means identification name = IMO = 9347279
        public string TransportMeanIdentification { get; set; }
        // CODE LIST IDENTIFICATION CODE = 146
        public string CodeListIdentificationTwo { get; set; }
        // Code list responsible agency code = 11
        public string CodeListResponsibleAgency { get; set; }
        // Transport ID. Function code qualifier = OKEE HENRI
        public string TransportIDName { get; set; }

        public override string ToEDIString()
        {
            if (string.IsNullOrEmpty(CarrierIdentifier) && string.IsNullOrEmpty(CarrierName))
            {

                return $"TDT+{(string.IsNullOrEmpty(TransportStage) ? "20" : TransportStage)}+{(string.IsNullOrEmpty(TransportMeansJourney) ? "" : TransportMeansJourney)}+{(string.IsNullOrEmpty(TransportModeName) ? "1" : TransportModeName)}++{CarrierIdentifier}:172:166+++{(string.IsNullOrEmpty(TransportMeanIdentification) ? "" : TransportMeanIdentification)}::{(string.IsNullOrEmpty(CodeListAgency) ? "11" : CodeListAgency)}:{TransportIDName}'";
            }
            return $"TDT+{(string.IsNullOrEmpty(TransportStage) ? "20" : TransportStage)}+{(string.IsNullOrEmpty(TransportMeansJourney) ? "" : TransportMeansJourney)}+{(string.IsNullOrEmpty(TransportModeName) ? "1" : TransportModeName)}++{CarrierIdentifier}:172:166:{CarrierName}+++{TransportMeanIdentification}:146:11:{TransportIDName}'";
        }
        public override string ToCustomEDI()
        {
            return $"";
        }
    }
}