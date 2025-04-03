using System;
using System.Collections.Generic;
using ReadEDIFACT.Models; // Replace 'YourNamespace' with the actual namespace where DataType is defined
using System.Linq;
using System.Threading.Tasks;

namespace ReadEDIFACT
{
    public class DataElement : Element
    {
        public DataType DataType { get; set; }
        public object? Precision { get; set; }
        public string JsonProperty { get; set; } = default!;

        public override string ToString()
        {
            string precisionOutput = Precision is int length ? length.ToString() :
                                    Precision is int[] range ? $"{range[0]}..{range[1]}" :
                                    "N/A";

            return $"{Name} ({Usage}) Type: {DataType}({precisionOutput})";
        }

    }
}