using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReadEDIFACT.Models.Coarri
{
    public class NAD: EDISegment
    {
        public string PartyQualifier { get; set; }
        public string PartyIdentification { get; set; }

        public override string ToEDIString()
        {
            return $"NAD+{PartyQualifier}+{PartyIdentification}'";
        }

        public override string ToCustomEDI()
        {
            return $"NAD+{PartyQualifier}+{PartyIdentification}'";
    }
    }
}