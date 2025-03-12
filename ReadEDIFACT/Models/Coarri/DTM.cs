using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReadEDIFACT.Models.Coarri
{
    public class DTM:EDISegment
    {
        public string DateOrTimeQualifier { get; set; }
        public string DateOrTime { get; set; }
        public string DateOrTimeFormatQualifier { get; set; }

        public override string ToEDIString()
        {
            return $"DTM+{DateOrTimeQualifier = "137"}:{(DateOrTime = string.IsNullOrEmpty(DateOrTime) ? DateTime.UtcNow.ToString("yyyyMMddHHmm") : DateOrTime)}:{DateOrTimeFormatQualifier = "203"}'";
        }  
    }
}