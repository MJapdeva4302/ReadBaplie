using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReadEDIFACT.Models.Coarri
{
    public class ArrivalData
    {
        public int ArrivalId { get; set; }
        public int ArrivalNumber { get; set; }
        public string IMO { get; set; }
        public string ShipName { get; set; }
        public string ETA { get; set; }
        public string ETD { get; set; }
        public string ShippingLine { get; set; }
        public string ShippingLineIdentification { get; set; }
        public string ShippingLineMail { get; set; }
    }
}