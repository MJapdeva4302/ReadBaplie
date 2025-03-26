using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Linq;
using System.Threading.Tasks;

namespace ReadEDIFACT.Models.Coarri
{
    public class EquipmentData
    {
        [JsonPropertyName("ContainerNumber")]
        public string? ContainerNumber { get; set; }

        [JsonPropertyName("BillOfLading")]
        public string? BillOfLading { get; set; }

        [JsonPropertyName("OperationType")]
        public int OperationType { get; set; }

        [JsonPropertyName("Condition")]
        public int Condition { get; set; }

        [JsonPropertyName("Dua")]
        public string? Dua { get; set; }

        [JsonPropertyName("LoadingUnloadingDate")]
        public string? LoadingUnloadingDate { get; set; }

        [JsonPropertyName("StowagePosition")]
        public int StowagePosition { get; set; }

        [JsonPropertyName("LoadingPort")]
        public string? LoadingPort { get; set; }

        [JsonPropertyName("DischargePort")]
        public string? DischargePort { get; set; }

        [JsonPropertyName("VerifyGrossMass")]
        public double VerifyGrossMass { get; set; }

        [JsonPropertyName("Weight")]
        public double Weight { get; set; }

        [JsonPropertyName("Temperature")]
        public string? Temperature { get; set; }

        [JsonPropertyName("TemperatureUnit")]
        public string? TemperatureUnit { get; set; }

        [JsonPropertyName("HazardousCode")]
        public int HazardousCode { get; set; }

        [JsonPropertyName("TransportStageCode")]
        public int TransportStageCode { get; set; }

        [JsonPropertyName("WarehouseDestiny")]
        public string? WarehouseDestiny { get; set; }

        [JsonPropertyName("CarrierIdentification")]
        public string? CarrierIdentification { get; set; }

        [JsonPropertyName("LocationFunctionPortCode")]
        public int LocationFunctionPortCode { get; set; }

        [JsonPropertyName("Seals")]
        public List<Seal>? Seals { get; set; }

        public List<Equipment>? Equipments { get; set; }
    }
}