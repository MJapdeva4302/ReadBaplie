using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReadEDIFACT.Models.Coarri
{
    public class DGS:EDISegment
    {
        // Código de la mercancía peligrosa; IMD = IMO  Codigo IMDG
        public string DangerousGoodsCode { get; set; }
        // Descripción de la mercancía peligrosa; Ejemplo: 3.1 NOTA: Clase 3 Líquidos inflamables
        public string HazardIdentificationCode { get; set; }
        // Información UNDG: Número ONU.
        public string DangerousGoodsClassificationCode { get; set; }
        public override string ToCustomEDI()
        {
            return $"DGS+{(string.IsNullOrEmpty(DangerousGoodsCode) ? "IMD" : DangerousGoodsCode)}+{HazardIdentificationCode}+{(string.IsNullOrEmpty(DangerousGoodsClassificationCode) ? "9999" : DangerousGoodsClassificationCode)}'";
        }

        public override string ToEDIString()
        {
            // Implement the logic for ToEDIString
            return string.Empty;
        }
    }
}