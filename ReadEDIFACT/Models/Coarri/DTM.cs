using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReadEDIFACT.Models.Coarri
{
    public class DTM : EDISegment
    {
        //178 = ATA = Actual Time of Arrival
        //186 = ATD = Actual Time of Departure
        public string DateOrTimeQualifier { get; set; }
        public string DateOrTime { get; set; }
        public string DateOrTimeFormatQualifier { get; set; }
        public BGM BGM { get; set; }


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
            if (BGM != null && BGM.DocumentName.Contains("119"))
            {
                return $"DTM+{DateOrTimeQualifier = "178"}:{(DateOrTime = string.IsNullOrEmpty(DateOrTime) ? DateTime.UtcNow.ToString("yyyyMMdd") : DateOrTime)}:{DateOrTimeFormatQualifier = "102"}'";
            }
            else if (BGM != null && BGM.DocumentName.Contains("122"))
            {
                return $"DTM+{DateOrTimeQualifier = "186"}:{(DateOrTime = string.IsNullOrEmpty(DateOrTime) ? DateTime.UtcNow.ToString("yyyyMMdd") : DateOrTime)}:{DateOrTimeFormatQualifier = "102"}'";
            }
            return $"";
        }
    }
}