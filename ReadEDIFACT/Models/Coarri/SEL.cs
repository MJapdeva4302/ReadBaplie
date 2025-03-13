using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReadEDIFACT.Models.Coarri
{
    public class SEL: EDISegment
    {
        public string SealNumber { get; set; }

        public override string ToEDIString()
        {
            return $"SEL+{SealNumber}+CA+1'";
        }

        public override string ToCustomEDI()
        {
            return $"SEL+{SealNumber}+CA+1'";
        }
    }
}