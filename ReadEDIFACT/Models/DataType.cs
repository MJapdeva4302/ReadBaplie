using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReadEDIFACT.Models
{
    public enum DataType
    {
        Alphabetic,   // Solo letras (A-Z, a-z)
        Alphanumeric, // Letras y números (A-Z, a-z, 0-9)
        Numeric,      // Solo números (0-9)
        Other,         // Otros tipos (sin validación específica)
        Decimal        // Numeros decimales (6.5)
    }
}