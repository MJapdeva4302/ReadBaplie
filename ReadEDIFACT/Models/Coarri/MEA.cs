using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Globalization;

namespace ReadEDIFACT.Models.Coarri
{
    public class MEA: EDISegment
    {
        // AAE = MEASUREMENT
        public string? MeasurementQualifier { get; set; }
        // VGM = Verified Gross Mass = Peso verificado y certificado del contenedor; G = Gross Weight
        public string? MeasurementAttribute { get; set; }
        // KGM = Kilogram; LBR = Pound
        public string? WeightUnitCode { get; set; }
        // 10000 = 10,00; el valor solo debe pemritir dos decimales; Peso bruto del contenedor, embalaje y mercancía. Dos decimales. 
        public double  MeasurementValue { get; set; }

        public override string ToEDIString()
        {
            return $"MEA+{MeasurementQualifier}+{MeasurementAttribute}+{WeightUnitCode}:{MeasurementValue.ToString("F2", CultureInfo.InvariantCulture)}'";
        }

        public override string ToCustomEDI()
        {
            return $"MEA+{(string.IsNullOrEmpty(MeasurementQualifier) ? "AAE" : MeasurementQualifier)}+VGM+KGM:{MeasurementValue}'";
        }
    }
}