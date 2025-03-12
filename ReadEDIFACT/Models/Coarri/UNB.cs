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

            return $"UNB+{(string.IsNullOrEmpty(SyntaxIdentifier) ? "UNOA" : SyntaxIdentifier)}:{(string.IsNullOrEmpty(SyntaxVersion) ? "2" : SyntaxVersion)}+{(string.IsNullOrEmpty(SenderIdentification) ? "" : SenderIdentification)}+{(string.IsNullOrEmpty(ReceiverIdentification) ? "" : ReceiverIdentification)}+{(string.IsNullOrEmpty(Date) ? "" : Date)}:{(string.IsNullOrEmpty(Time) ? "" : Time)}+{InterchangeRef}'";
        }
    }
}