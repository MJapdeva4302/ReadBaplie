using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReadEDIFACT.Models.Coarri
{
    public class CNT: EDISegment
    {
        public string ControlTotalQualifier { get; set; }
        public string ControlTotalValue { get; set; }

        public override string ToEDIString()
        {
            return $"CNT+{ControlTotalQualifier}+{ControlTotalValue}'";
        }

        public override string ToCustomEDI()
        {
            return $"CNT+{ControlTotalQualifier}+{ControlTotalValue}'";
        }
    }
}