using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Linq;
using System.Threading.Tasks;

namespace ReadEDIFACT.Models.Coarri
{
    public class RootData
    {
        // public ArrivalData ArrivalData { get; set; }
        // public List<Equipment> Equipments { get; set; } 

        [JsonPropertyName("ArrivalData")]
        public ArrivalDataJson ArrivalData { get; set; }

        [JsonPropertyName("Equipments")]
        public List<EquipmentData> Equipments { get; set; }
    }
}