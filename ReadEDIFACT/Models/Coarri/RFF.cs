using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReadEDIFACT.Models.Coarri
{
    public class RFF:EDISegment
    {
        public string ReferenceQualifier { get; set; } 
        public string ReferenceIdentifier { get; set; }
        public EQD EQD { get; set; }
        public BGM BGM { get; set; }

        public override string ToEDIString()
        {

            return $"RFF+{(String.IsNullOrEmpty(ReferenceQualifier) ? "VM" : ReferenceIdentifier)}:{ReferenceIdentifier}'";
        }

        public override string ToCustomEDI()
        {
            if (EQD == null)
            {
                return $"";
            }
            else
            {
                // BM Guía de transporte; Número de Guía de transporte
                if (EQD.FullEmptyIndicator.Equals("5") && ReferenceQualifier.Equals("BM"))
                {
                    return $"RFF+{ReferenceQualifier}:{ReferenceIdentifier}'";
                }
                // BN Número de reserva asignado por la línea naviera;  Número de reserva
                else if (EQD.FullEmptyIndicator.Equals("4") && ReferenceQualifier.Equals("BN"))
                {
                    return $"RFF+{ReferenceQualifier}:{ReferenceIdentifier}'";
                }
                // ABT: Número de declaración de exportación emitido por las aduanas de Costa Rica; Número de Declaración de Exportación
                else
                {
                    return $"RFF+{ReferenceQualifier}:{ReferenceIdentifier}'";
                }
            }
        }

        public string ReturnFormat(string ReferenceQualifier, string ReferenceIdentifier)
        {
            this.ReferenceQualifier = ReferenceQualifier;
            this.ReferenceIdentifier = ReferenceIdentifier;
            return $"RFF+{ReferenceQualifier}:{ReferenceIdentifier}'";
        }
    }
}