using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReadEDIFACT.Models.Coarri
{
    public class UNH : EDISegment
    {
        public string MessageRefNumber {get; set;}
        public string MessageTypeId { get; set; }
        public string MessageTypeVersion { get; set; }
        public string MessageTypeRelease { get; set; }
        public string AssociationAssigned { get; set; }
        public string AssociationAssigned { get; set; }

        public override string ToEDIString()
        {
            return $"UNH+{(MessageRefNumber = MessageRefNumber == "" || MessageRefNumber == null ? "COARRI" : MessageRefNumber)}+{()}'";
        }
    }
}