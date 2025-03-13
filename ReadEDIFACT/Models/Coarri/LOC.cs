using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReadEDIFACT.Models.Coarri
{
    public class LOC : EDISegment
    {
        public string LocationQualifier { get; set; }
        public string LocationCode { get; set; }


        public override string ToEDIString()
        {
            return $"LOC+{LocationQualifier}+{LocationCode}:139:5'";
        }
        
        public override string ToCustomEDI()
        {
            // Implement the method according to your requirements
            return $"LOC+{LocationQualifier}+{LocationCode}:139:5'";
        }
    }
}