using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReadEDIFACT.Models.Coarri
{
    public class Equipment
    {
        public EQD? EquipmentDetails { get; set; }
        public RFF? Reference1 { get; set; }
        public RFF? Reference2 { get; set; }
        public DTM? Date { get; set; }
        // public List<LOC> Location { get; set; }
        // 147 = Localizacion de la estiba
        public LOC? Location1 { get; set; }
        // 9 = Lugar de procedencia / carga
        public LOC? Location2 { get; set; }
        // 11 = Lugar de descarga
        public LOC? Location3 { get; set; }
        public MEA? Measurements { get; set; }
        public TMP? Temperature { get; set; }
        public List<SEL>? Seals { get; set; }
        public DGS? DangerousGoods { get; set; }
        public FTX? FreeText { get; set; }
        public NAD? Parties { get; set; }
    }
}