using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReadEDIFACT.Models.Coarri
{
    public class DTM : EDISegment
    {
        public string DateOrTimeQualifier { get; set; }
        public string DateOrTime { get; set; }
        public string DateOrTimeFormatQualifier { get; set; }


        public string ReturnFormat(string? ETA = null, string? ETD = null)
        {
            if (ETA != null)
            {
                DateOrTimeQualifier = "132";
                DateOrTime = ETA;
                DateOrTimeFormatQualifier = "203";
            }
            else if (ETD != null)
            {
                DateOrTimeQualifier = "133";
                DateOrTime = ETD;
                DateOrTimeFormatQualifier = "203";
            }
            return $"DTM+{DateOrTimeQualifier}:{DateOrTime}:{DateOrTimeFormatQualifier}'";
        }

        public override string ToEDIString()
        {
            return $"DTM+{DateOrTimeQualifier = "137"}:{(DateOrTime = string.IsNullOrEmpty(DateOrTime) ? DateTime.UtcNow.ToString("yyyyMMddHHmm") : DateOrTime)}:{DateOrTimeFormatQualifier = "203"}'";
        }

        public override string ToCustomEDI()
        {
            return $"";
        }
    }
}