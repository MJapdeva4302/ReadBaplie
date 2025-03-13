using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReadEDIFACT.Models.Coarri
{
    public class UNZ: EDISegment
    {
        public string InterchangeControlCount { get; set; }
        public string InterchangeRef { get; set; }

        public override string ToEDIString()
        {
            return $"UNZ+{InterchangeControlCount}+{InterchangeRef}'";
        }

        public override string ToCustomEDI()
        {
            return $"UNZ+{InterchangeControlCount}+{InterchangeRef}'";
        }
    }
}