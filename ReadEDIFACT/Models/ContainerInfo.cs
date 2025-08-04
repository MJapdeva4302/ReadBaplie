using System;

namespace ReadEDIFACT.Models
{
    public class ContainerInfo
    {
        public string ContainerNumber { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string Size { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string Weight { get; set; } = string.Empty;
        public string PortOfDischarge { get; set; } = string.Empty;
        public string PortOfLoading { get; set; } = string.Empty;
    }

    public class VesselInfo
    {
        public string VesselName { get; set; } = string.Empty;
        public string VoyageNumber { get; set; } = string.Empty;
        public string VesselOperator { get; set; } = string.Empty;
    }
}
