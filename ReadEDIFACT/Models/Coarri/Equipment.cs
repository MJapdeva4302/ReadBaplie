using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReadEDIFACT.Models.Coarri
{
    public class Equipment
    {
        public EQD EquipmentDetails { get; set; }
        public string TripIdentificationNumber { get; set; }
        public string LoadingUnloadingDate { get; set; }
        public int StowagePosition { get; set; }
        public string LoadingPort { get; set; }
        public string DischargePort { get; set; }
        public double VerifyGrossMass { get; set; }
        public double Weight { get; set; }
        public string Temperature { get; set; }
        public string TemperatureUnit { get; set; }
        public int HazardousCode { get; set; }
        public int TransportStageCode { get; set; }
        public string WarehouseDestiny { get; set; }
        public string CarrierIdentification { get; set; }
        public int LocationFunctionPortCode { get; set; }
        public List<Seal> Seals { get; set; }
    }
}