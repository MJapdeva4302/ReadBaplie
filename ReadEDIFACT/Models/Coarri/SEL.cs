using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReadEDIFACT.Models.Coarri
{
    public class SEL: EDISegment
    {
        // Número de sello; Ejemplo: 123456
        public string SealNumber { get; set; }
        // Tipo de sello; Ejemplo: CA; AA = consolidador AB = desconocido AC = Agencia cuarentenaria CA = Naviera CU = Aduanas SH = Exportador TO = Operador portuario
        public string SealPartyNameCode { get; set; }
        // Cantidad de sellos; Ejemplo: 1   
        public string SealType { get; set; }

        public override string ToEDIString()
        {
            return $"SEL+{(string.IsNullOrEmpty(SealNumber) ? "" : SealNumber)}+{(string.IsNullOrEmpty(SealPartyNameCode) ? "AA" : SealPartyNameCode)}+{(string.IsNullOrEmpty(SealType) ? "1" : SealType)}'";
        }

        public override string ToCustomEDI()
        {
            return $"SEL+{SealNumber}+CA+1'";
        }
    }
}