using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReadEDIFACT.Models.Coarri
{
    public class RFF:EDISegment
    {
        public string ReferenceQualifier { get; set; } 
        public string ReferenceIdentifier { get; set; }

        public override string ToEDIString()
        {

            return $"RFF+{(String.IsNullOrEmpty(ReferenceQualifier) ? "VM" : ReferenceIdentifier)}:{ReferenceIdentifier}'";
        }

        public override string ToCustomEDI()
        {
            return $"";
        }

        public string ReturnFormat(string ReferenceQualifier, string ReferenceIdentifier)
        {
            this.ReferenceQualifier = ReferenceQualifier;
            this.ReferenceIdentifier = ReferenceIdentifier;
            return $"RFF+{ReferenceQualifier}:{ReferenceIdentifier}'";
        }
    }
}