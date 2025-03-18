using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReadEDIFACT.Models.Coarri
{
    public class Equipment
    {
        public EQD EquipmentDetails { get; set; }
        public RFF Reference { get; set; }
        public DTM Date { get; set; }
        // public List<LOC> Location { get; set; }
        public LOC Location { get; set; }
        public MEA Measurements { get; set; }
        public TMP Temperature { get; set; }
        public List<SEL> Seals { get; set; }
        public DGS DangerousGoods { get; set; }
        public FTX FreeText { get; set; }
    }
}