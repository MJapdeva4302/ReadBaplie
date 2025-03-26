using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReadEDIFACT
{
    public class SegmentData : Segment
    {        
        public String? Purpose { get; set; }        
        public int? MaxUse { get; set; }        
        public string? Notes { get; set; }
        public IEnumerable<Element>? DataElements { get; set; }

        public override string ToString()
        {
            return string.Format(base.ToString());
        }
    }
}