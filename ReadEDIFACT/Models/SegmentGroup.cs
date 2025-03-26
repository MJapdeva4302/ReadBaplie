using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReadEDIFACT
{
    public class SegmentGroup : Segment
    {
        public int? GroupRepeat { get; set; }
        public IEnumerable<Segment>? Segments { get; set; }
    }

}