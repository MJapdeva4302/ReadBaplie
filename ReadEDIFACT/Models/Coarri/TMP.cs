using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReadEDIFACT.Models.Coarri
{
    public class TMP:EDISegment
    {
        public string TemperatureQualifier { get; set; }
        public string Temperature { get; set; }

        public override string ToEDIString()
        {
            return $"TMP+{TemperatureQualifier}+{Temperature}'";
        }

        public override string ToCustomEDI()
        {
            return $"TMP+{TemperatureQualifier}+{Temperature}'";
        }
    }
}