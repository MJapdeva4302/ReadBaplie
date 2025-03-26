using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Linq;
using System.Threading.Tasks;

namespace ReadEDIFACT.Models.Coarri
{
    public class Seal
    {
        [JsonPropertyName("SealNumber")]
        public string? SealNumber { get; set; }

        [JsonPropertyName("SealType")]
        public string? SealType { get; set; }

        [JsonPropertyName("SealPartyNameCode")]
        public string? SealPartyNameCode { get; set; }
    }
}