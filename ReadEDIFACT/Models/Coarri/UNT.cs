using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReadEDIFACT.Models.Coarri
{
    public class UNT: EDISegment
    {
        // Número de segmentos dentro del mensaje, excluyendo los segmentos UnA, UNB y UNZ e incluyendo UNH y UNT
        public string SegmentCount { get; set; }
        // Número de referencia del mensaje. Debe ser idéntico al número en el capo 0062 del segment UNH.
        public string MessageRef { get; set; }

        public override string ToEDIString()
        {
            return $"UNT+{SegmentCount}+{MessageRef}'";
        }

        public override string ToCustomEDI()
        {
            return $"UNT+{SegmentCount}+{MessageRef}'";
        }
    }
}