using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReadEDIFACT.Models.Coarri
{
    public class FTX : EDISegment
    {
        // AAD = Descripción de mercancía peligrosa
        public string? TextSubjectCode { get; set; }
        // Descripción de la mercancía peligrosa
        public string? TextValue { get; set; }
        public DGS? DGS { get; set; }
        public override string ToEDIString()
        {
            if (DGS != null)
            {
                return $"FTX+{(string.IsNullOrEmpty(TextSubjectCode) ? "AAD" : TextSubjectCode)}+++{TextValue}'";
            }
            // Provide implementation for ToEDIString
            return $"";
        }

        public override string ToCustomEDI()
        {
            // Provide implementation for ToCustomEDI
            return "FTX segment in custom EDI format";
        }
    }
}