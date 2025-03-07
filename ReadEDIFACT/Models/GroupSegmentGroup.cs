using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReadEDIFACT.Models
{
    public class GroupSegmentGroup: SegmentGroup
    {
        public int? GroupRepeat { get; set; }
        public IEnumerable<SegmentGroup> GroupSegments { get; set; }
    }
}