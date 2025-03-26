using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReadEDIFACT.Models.Coarri
{
    public class TMD:EDISegment
    {
        public string? TransportMode { get; set; }

        public override string ToEDIString()
        {
            return $"TMD+{TransportMode}'";
        }

        public override string ToCustomEDI()
        {
            return $"TMD+{TransportMode}'";
    }
    }
}