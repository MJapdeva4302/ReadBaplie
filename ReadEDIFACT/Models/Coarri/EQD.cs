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
        public string EquipmentSizeAndType { get; set; }
        // Code identifying the type of equipment = 102
        public string CodeListIdentification { get; set; }
        // Code list responsible agency, coded = 5
        public string CodeListResponsibleAgency { get; set; }

        // Equipment status code: 2 = Export, 3 = Import
        public string EquipmentStatusCode { get; set; }
        // Full/empty indicator =  Full=5, Empty=4
        public string FullEmptyIndicator { get; set; }


        public override string ToEDIString()
        {
            if (!string.IsNullOrEmpty(EquipmentQualifier) && FullEmptyIndicator.Equals("4"))
            {
                return $"EQD+{EquipmentQualifier}+{ContainerNumber}+{EquipmentSizeAndType}+++{FullEmptyIndicator}'";
            }
            else
            {
                return $"EQD+{EquipmentQualifier}+{ContainerNumber}+{EquipmentSizeAndType}:{(string.IsNullOrEmpty(CodeListIdentification) ? "102" : CodeListIdentification)}:{(string.IsNullOrEmpty(CodeListResponsibleAgency) ? "5" : CodeListResponsibleAgency)}++{(string.IsNullOrEmpty(EquipmentStatusCode) ? "2" : EquipmentStatusCode)}+{FullEmptyIndicator}'";
            }
        }

        public override string ToCustomEDI()
        {
            return $"EQD+{EquipmentQualifier}+{ContainerNumber}+{EquipmentSizeAndType}:102:5++2+5'";
        }
    }
}