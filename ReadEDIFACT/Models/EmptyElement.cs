using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReadEDIFACT
{
    public class EmptyElement : Element
    {
        public override string ToString()
        {
            return "Empty element";
        }
    }
}