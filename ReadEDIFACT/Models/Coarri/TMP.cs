using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Globalization;

namespace ReadEDIFACT.Models.Coarri
{
    public class TMP:EDISegment
    {
        // 2 = Temperatura del contenedor
        public string TemperatureQualifier { get; set; }
        // 1.4 = valor de la temperatura
        public double TemperatureValue { get; set; }
        // CEL = Celsius; FAH = Fahrenheit
        public string TemperatureUnit { get; set; }

        public override string ToEDIString()
        {
            return $"TMP+{(string.IsNullOrEmpty(TemperatureQualifier) ? "2" : TemperatureQualifier)}+{TemperatureValue.ToString("F1", CultureInfo.InvariantCulture)}:{(string.IsNullOrEmpty(TemperatureUnit) ? "CEL" : TemperatureUnit)}'";
        }

        public override string ToCustomEDI()
        {
            return $"TMP+{TemperatureQualifier}+{TemperatureValue}'";
        }
    }
}