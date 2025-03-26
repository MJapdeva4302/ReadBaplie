using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReadEDIFACT.Models.Coarri
{
    public class ArrivalData
    {
        public UNB? InterchangeHeader { get; set; }
        public UNH? MessageHeader { get; set; }
        public BGM? BeginningOfMessage { get; set; }
        public TDT? TransportInformation { get; set; }
        public RFF? Reference { get; set; }
        public LOC? Location1 { get; set; }
        public LOC? Location2 { get; set; }
        public LOC? Location3 { get; set; }
        public LOC? Location4 { get; set; }
        public LOC? Location5 { get; set; }
        public DTM? Date1 { get; set; }
        public DTM? Date2 { get; set; }
        public NAD? Parties { get; set; }

    }
}