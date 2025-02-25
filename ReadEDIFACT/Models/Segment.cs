using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReadEDIFACT
{
    public abstract class Segment
    {
        public String SegmentID { get; set; }
        public String Name { get; set; }
        public int Position { get; set; }
        public RuleUsage Usage { get; set; }        
        public RuleUsage DirUsage { get; set; }

        public override string ToString()
        {
            return string.Format("{0}:{1} ({2})", SegmentID, Name, Usage);
        }
    }
}