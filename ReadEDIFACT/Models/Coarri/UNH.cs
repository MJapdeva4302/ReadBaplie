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
            return $"UNH+{(string.IsNullOrEmpty(MessageRefNumber) ? GenerateInterchangeRef() : MessageRefNumber)}+{(MessageTypeId = MessageTypeId == "" || MessageTypeId == null ? "COARRI" : MessageTypeId)}:{(MessageTypeVersion = MessageTypeVersion == "" || MessageTypeVersion == null ? "D" : MessageTypeVersion)}:{(MessageTypeRelease = MessageTypeRelease == "" || MessageTypeRelease == null ? "23A" : MessageTypeRelease)}:{(ControllingAgency = ControllingAgency == "" || ControllingAgency == null ? "UN" : ControllingAgency)}+{(AssociationAssigned = AssociationAssigned == "" || AssociationAssigned == null ? "ITG10" : AssociationAssigned)}'";
        }

        public override string ToCustomEDI()
        {
            throw new NotImplementedException();
        }

        public static string GenerateInterchangeRef()
        {
            string timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmss");

            if (timestamp.Length > 5)
            {
                timestamp = timestamp.Substring(timestamp.Length - 5);
            }

            Random random = new Random();
            
            string randomNumber = random.Next(100, 999).ToString(); 

            
            string interchangeRef = $"{timestamp}{randomNumber}";

           
            if (interchangeRef.Length > 8)
            {
                interchangeRef = interchangeRef.Substring(0, 8);
            }

            return interchangeRef;
        }
    }
}