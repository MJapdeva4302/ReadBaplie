using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReadEDIFACT
{
    public abstract class Element
    {
        public int ElementNumber { get; set; }
        public string? Identifier { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Notes { get; set; }
        public RuleUsage Usage { get; set; }
    }
}