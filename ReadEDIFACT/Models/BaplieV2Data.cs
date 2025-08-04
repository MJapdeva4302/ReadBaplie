using System;
using System.Collections.Generic;

namespace ReadEDIFACT.Models
{
    public class BaplieV2RootData
    {
        public BaplieV2ArrivalData ArrivalData { get; set; } = new BaplieV2ArrivalData();
        public List<BaplieV2Equipment> Equipments { get; set; } = new List<BaplieV2Equipment>();
    }

    public class BaplieV2ArrivalData
    {
        public string VesselName { get; set; } = "";
        public string IMO { get; set; } = "";
        public string CallSign { get; set; } = "";
        public string VoyageNumber { get; set; } = "";
        public string ETA { get; set; } = "";
        public string ETD { get; set; } = "";
        public string LoadingPortCode { get; set; } = "";
        public string LoadingPortName { get; set; } = "";
        public string DischargePortCode { get; set; } = "";
        public string DischargePortName { get; set; } = "";
        public string DestinationPort { get; set; } = "";
        public string DestinationPortName { get; set; } = "";
        public string ShippingLine { get; set; } = "";
        public string ShippingLineIdentification { get; set; } = "";

        // Segmentos EDI
        public SegmentData? InterchangeHeader { get; set; }
        public SegmentData? MessageHeader { get; set; }
        public SegmentData? BeginningOfMessage { get; set; }
        public SegmentData? TransportInformation { get; set; }
        public SegmentData? Reference { get; set; }
        public List<SegmentData> Locations { get; set; } = new List<SegmentData>();
        public List<SegmentData> DateTimes { get; set; } = new List<SegmentData>();
        public SegmentData? Parties { get; set; }
    }

    public class BaplieV2Equipment
    {
        public string ContainerNumber { get; set; } = "";
        public string StowagePosition { get; set; } = "";
        public string Size { get; set; } = "";
        public string Type { get; set; } = "";
        public string LoadingPort { get; set; } = "";
        public string DischargePort { get; set; } = "";
        public decimal Weight { get; set; }
        public string PODStatus { get; set; } = "";
        public string MovementType { get; set; } = "";
        
        // Segmentos EDI
        public SegmentData? EquipmentDetails { get; set; }
        public List<SegmentData> References { get; set; } = new List<SegmentData>();
        public SegmentData? Date { get; set; }
        public List<SegmentData> Locations { get; set; } = new List<SegmentData>();
        public SegmentData? Measurements { get; set; }
        public List<SegmentData>? AdditionalInformation { get; set; }
    }
}
