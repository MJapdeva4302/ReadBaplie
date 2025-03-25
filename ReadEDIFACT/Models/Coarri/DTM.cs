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


        public string ReturnFormat(string data, string qualifier)
        {
            if (data.Contains("132"))
            {
                DateOrTimeQualifier = "132";
                DateOrTime = qualifier;
                DateOrTimeFormatQualifier = "203";
                return $"DTM+{DateOrTimeQualifier}:{DateOrTime}:{DateOrTimeFormatQualifier}'";
            }
            else if (data.Contains("133"))
            {
                DateOrTimeQualifier = "133";
                DateOrTime = qualifier;
                DateOrTimeFormatQualifier = "203";
                return $"DTM+{DateOrTimeQualifier}:{DateOrTime}:{DateOrTimeFormatQualifier}'";
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

        public string DateOperation(string dateOrTime)
        {
            return $"DTM+203:{dateOrTime}:203'";
        }
    }
}