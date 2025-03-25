using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Linq;
using System.Threading.Tasks;

namespace ReadEDIFACT.Models.Coarri
{
    public class ArrivalDataJson
    {
        [JsonPropertyName("ArrivalId")]
        public int ArrivalId { get; set; }

        [JsonPropertyName("ArrivalNumber")]
        public int ArrivalNumber { get; set; }

        [JsonPropertyName("IMO")]
        public int IMO { get; set; }

        [JsonPropertyName("ShipName")]
        public string ShipName { get; set; }

        [JsonPropertyName("CallSign")]
        public string CallSign { get; set; }

        [JsonPropertyName("ETA")]
        public string ETA { get; set; }

        [JsonPropertyName("ETD")]
        public string ETD { get; set; }

        [JsonPropertyName("LoadingPortCode")]
        public string LoadingPortCode { get; set; }

        [JsonPropertyName("LoadingPortName")]
        public string LoadingPortName { get; set; }

        [JsonPropertyName("DischargePortCode")]
        public string DischargePortCode { get; set; }

        [JsonPropertyName("DischargePortName")]
        public string DischargePortName { get; set; }

        [JsonPropertyName("OriginPort")]
        public string OriginPort { get; set; }

        [JsonPropertyName("OriginPortName")]
        public string OriginPortName { get; set; }

        [JsonPropertyName("DestinationPort")]
        public string DestinationPort { get; set; }

        [JsonPropertyName("DestinationPortName")]
        public string DestinationPortName { get; set; }

        [JsonPropertyName("ShippingLine")]
        public string ShippingLine { get; set; }

        [JsonPropertyName("ShippingLineIdentification")]
        public string ShippingLineIdentification { get; set; }

        [JsonPropertyName("ShippingLineAddress")]
        public string ShippingLineAddress { get; set; }

        [JsonPropertyName("ShippingLineMail")]
        public string ShippingLineMail { get; set; }

        [JsonPropertyName("DocNameCode")]
        public int DocNameCode { get; set; }

        [JsonPropertyName("LocFunctionCode")]
        public int LocFunctionCode { get; set; }

        public ArrivalData arrivalData { get; set; }
    }
}