using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReadEDIFACT.Models.Coarri
{
    public class UNB :EDISegment
    {
        public string SyntaxIdentifier { get; set; }
        public string SyntaxVersion {get; set;}
        public string SenderIdentification { get; set; }
        public string ReceiverIdentification { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
        public string InterchangeRef { get; set; }

        public override string ToEDIString()
        {

            return $"UNB+{(SyntaxIdentifier = !SyntaxIdentifier.Equals("UNOA") ? "UNOA" : SyntaxIdentifier)}:{(SyntaxVersion = !SyntaxVersion.Equals("2") ? "2" : SyntaxVersion)}+{(SenderIdentification = SenderIdentification == "" || SenderIdentification == null ? "" : SenderIdentification)}+{(ReceiverIdentification = ReceiverIdentification != "" ? ReceiverIdentification : "")}+{(Date = Date != null ? Date : "")}:{(Time = Time != null ? Time : "")}+{InterchangeRef}'";
        }
    }
}