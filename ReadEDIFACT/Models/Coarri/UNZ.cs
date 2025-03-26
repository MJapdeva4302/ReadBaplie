using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReadEDIFACT.Models.Coarri
{
    public class UNZ: EDISegment
    {
        // Conteo de control de intercambio: Número de mensajes en el intercambio.
        public string? InterchangeControlCount { get; set; }
        // Referencia de control de intercambio: esta referencia debe ser idéntica a la que se encuentra en UNB campo 0020.
        public string? MessageRef { get; set; }

        public override string ToEDIString()
        {
            return $"UNZ+{InterchangeControlCount}+{MessageRef}'";
        }

        public override string ToCustomEDI()
        {
            return $"UNZ+{InterchangeControlCount}+{MessageRef}'";
        }
    }
}