using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReadEDIFACT.Models.Coarri
{
    public class UNT: EDISegment
    {
        public string SegmentCount { get; set; }
        public string MessageRef { get; set; }

        public override string ToEDIString()
        {
            return $"UNT+{SegmentCount}+{MessageRef}'";
        }

        public override string ToCustomEDI()
        {
            return $"UNT+{SegmentCount}+{MessageRef}'";
        }
    }
}