using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReadEDIFACT.Models.Coarri
{
    public class ArrivalData
    {
        public SegmentData? InterchangeHeader { get; set; }  // UNB
        public SegmentData? MessageHeader { get; set; }      // UNH
        public SegmentData? BeginningOfMessage { get; set; } // BGM
        public SegmentData? TransportInformation { get; set; } // TDT
        public SegmentData? Reference { get; set; }          // RFF
        public List<SegmentData> Locations { get; set; } = new(); // LOC1-5
        public List<SegmentData> DateTimes { get; set; } = new(); // DTM
        public SegmentData? Parties { get; set; }            // NAD

    }
}