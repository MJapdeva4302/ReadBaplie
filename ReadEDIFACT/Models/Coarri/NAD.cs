using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReadEDIFACT.Models.Coarri
{
    public class NAD : EDISegment
    {
        // CA = Carrier, CF = Container, SLS = Shipper

        // Esto es del contenedor: CF = Naviera o agencia Naviera responsible del contenedor
        public string PartyQualifier { get; set; }
        // Carrier Code = Codigo de naviero
        public string PartyIdentifier { get; set; }
        // Code list identification code = 160
        public string CodeListIdentification { get; set; }
        // Code identifying the agency controlling the specification, maintenance and publication of the code list = 20, 166, ZZZ
        public string CodeListResponsibleAgency { get; set; }
        // Name of the party = CHIQUITA BRANDS
        public string PartyName { get; set; }

        public override string ToEDIString()
        {
            if (PartyQualifier.Equals("CA"))
            {
                return $"NAD+{PartyQualifier}+{(string.IsNullOrEmpty(PartyIdentifier) ? "" : PartyIdentifier)}:{(string.IsNullOrEmpty(CodeListIdentification) ? "160" : CodeListIdentification)}:{(string.IsNullOrEmpty(CodeListResponsibleAgency) ? "166" : CodeListResponsibleAgency)}++{(string.IsNullOrEmpty(PartyName) ? "" : PartyName)}'";
            }
            else if (PartyQualifier.Equals("CF"))
            {
                return $"NAD+{PartyQualifier}+{(string.IsNullOrEmpty(PartyIdentifier) ? "" : PartyIdentifier)}:{(string.IsNullOrEmpty(CodeListIdentification) ? "160" : CodeListIdentification)}:{(string.IsNullOrEmpty(CodeListResponsibleAgency) ? "166" : CodeListResponsibleAgency)}++{(string.IsNullOrEmpty(PartyName) ? "" : PartyName)}'";
            }
            else
            {
                return $"NAD+{PartyQualifier}+{(string.IsNullOrEmpty(PartyIdentifier) ? "" : PartyIdentifier)}:{(string.IsNullOrEmpty(CodeListIdentification) ? "160" : CodeListIdentification)}:{(string.IsNullOrEmpty(CodeListResponsibleAgency) ? "166" : CodeListResponsibleAgency)}++{(string.IsNullOrEmpty(PartyName) ? "" : PartyName)}'";
            }
        }

        public override string ToCustomEDI()
        {
            return $"NAD+{(string.IsNullOrEmpty(PartyQualifier) ? "CF" : PartyQualifier)}+{(string.IsNullOrEmpty(PartyIdentifier) ? "" : PartyIdentifier)}:{(string.IsNullOrEmpty(CodeListIdentification) ? "160" : CodeListIdentification)}:{(string.IsNullOrEmpty(CodeListResponsibleAgency) ? "166" : CodeListResponsibleAgency)}'";
        }
    }
}