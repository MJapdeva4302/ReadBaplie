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
        public string ControllingAgency { get; set; }
        public string AssociationAssigned { get; set; }

        public override string ToEDIString()
        {
            return $"UNH+{(MessageRefNumber = MessageRefNumber == "" || MessageRefNumber == null ? "244172" : MessageRefNumber)}+{(MessageTypeId = MessageTypeId == "" || MessageTypeId == null ? "COARRI" : MessageTypeId)}:{(MessageTypeVersion = MessageTypeVersion == "" || MessageTypeVersion == null ? "D" : MessageTypeVersion)}:{(MessageTypeRelease = MessageTypeRelease == "" || MessageTypeRelease == null ? "23A" : MessageTypeRelease)}:{(ControllingAgency = ControllingAgency == "" || ControllingAgency == null ? "UN" : ControllingAgency)}+{(AssociationAssigned = AssociationAssigned == "" || AssociationAssigned == null ? "ITG10" : AssociationAssigned)}'";
        }

        public override string ToCustomEDI()
        {
            throw new NotImplementedException();
        }
    }
}