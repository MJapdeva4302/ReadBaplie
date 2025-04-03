using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReadEDIFACT.Models.Coarri
{
    public class Equipment
    {
        public SegmentData? EquipmentDetails { get; set; }   // EQD
        public List<SegmentData> References { get; set; } = new(); // RFF
        public SegmentData? Date { get; set; }               // DTM
        public List<SegmentData> Locations { get; set; } = new(); // LOC
        public SegmentData? Measurements { get; set; }       // MEA
        public SegmentData? Temperature { get; set; }        // TMP
        public List<SegmentData>? Seals { get; set; }        // SEL
        public SegmentData? DangerousGoods { get; set; }     // DGS
        public SegmentData? FreeText { get; set; }           // FTX
        public SegmentData? Parties { get; set; }            // NAD
    }
}