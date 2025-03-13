using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReadEDIFACT.Models.Coarri
{
    public class EQD: EDISegment
    {
        public string EquipmentQualifier { get; set; }
        public string ContainerNumber { get; set; }
        public string ContainerType { get; set; }

        public override string ToEDIString()
        {
            return $"EQD+{EquipmentQualifier}+{ContainerNumber}+{ContainerType}:102:5++2+5'";
        }

        public override string ToCustomEDI()
        {
            return $"EQD+{EquipmentQualifier}+{ContainerNumber}+{ContainerType}:102:5++2+5'";
        }
    }
}