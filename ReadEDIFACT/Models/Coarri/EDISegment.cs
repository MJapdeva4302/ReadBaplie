using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReadEDIFACT.Models.Coarri
{
    public abstract class EDISegment
    {
        public abstract string ToEDIString();
    }
}