using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReadEDIFACT.Models.Coarri
{
    public class BGM: EDISegment
    {
        public string? DocumentName { get; set; }
        public string? DocumentNumber { get; set; }
        public string? MessageFunction { get; set; }

        public override string ToEDIString()
        {
            if (DocumentName != null && DocumentName.Contains("119"))
            {
            DocumentName = "119";
            }
            else if (DocumentName != null && DocumentName.Contains("122"))
            {
            DocumentName = "122";
            }else{
            DocumentName = "270";
            }

            return $"BGM+{DocumentName}+{(DocumentNumber ?? "")}+{(MessageFunction ?? "9")}'";
        }

        public override string ToCustomEDI()
        {
            return $"";
        }
    }
}