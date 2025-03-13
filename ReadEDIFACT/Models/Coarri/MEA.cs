using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReadEDIFACT.Models.Coarri
{
    public class MEA: EDISegment
    {
        public string MeasurementQualifier { get; set; }
        public double Weight { get; set; }

        public override string ToEDIString()
        {
            return $"MEA+{MeasurementQualifier}+VGM+KGM:{Weight}'";
        }

        public override string ToCustomEDI()
        {
            return $"MEA+{MeasurementQualifier}+VGM+KGM:{Weight}'";
        }
    }
}