using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReadEDIFACT.Models.Coarri
{
    public class EQD : EDISegment
    {
        // Equipment type code qualifier = CN
        public string EquipmentQualifier { get; set; }

        // Equipment identifier = BEAU5199464
        public string ContainerNumber { get; set; }
        // Equipment size and type = 45G1
        public string ContainerType { get; set; }
        // Full/empty indicator =  Full=5, Empty=4
        public string FullEmptyIndicator { get; set; }


        public override string ToEDIString()
        {
            if (string.IsNullOrEmpty(EquipmentQualifier))
            {
                return $"EQD+{EquipmentQualifier}+{ContainerNumber}+{ContainerType}+++{FullEmptyIndicator}'";
            }
            else
            {
                return $"EQD+{EquipmentQualifier}+{ContainerNumber}+{ContainerType}+++{FullEmptyIndicator}'";
            }
        }

        public override string ToCustomEDI()
        {
            return $"EQD+{EquipmentQualifier}+{ContainerNumber}+{ContainerType}:102:5++2+5'";
        }
    }
}