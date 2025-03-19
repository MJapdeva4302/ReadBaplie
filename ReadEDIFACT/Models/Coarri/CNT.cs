using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReadEDIFACT.Models.Coarri
{
    public class CNT: EDISegment
    {
        // 16 = Número total de contenedores
        public string ControlTotalQualifier { get; set; }
        // Número total de contenedores: numero total de todos los contenedores en el mensaje
        public string ControlTotalValue { get; set; }

        public override string ToEDIString()
        {
            return $"CNT+{ControlTotalQualifier}:{ControlTotalValue}'";
        }

        public override string ToCustomEDI()
        {
            return $"CNT+{ControlTotalQualifier}:{ControlTotalValue}'";
        }
    }
}