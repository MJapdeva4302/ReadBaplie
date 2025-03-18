using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReadEDIFACT.Models.Coarri
{
    public class LOC : EDISegment
    {
        /*
        41 = aduanas de entrada en un reporte de descarga
        42 = aduanas de salida en un reporte de carga
        9 = carga
        11 = descarga
        9 = Puerto de salida
        94 = puerto de atraque previo
        61 = Proximo Puerto de llamada

        Esto pertenece al segmento LOC que esta dentro del EQD en el COARRI
        147 = Localizacion de la estiba
        9 = Lugar de procedencia / carga
        11 = Lugar de descarga
        */
        public string LocationQualifier { get; set; }
        // LOCODE = ECGYE = Ecuador - Guayaquil; si es 119 en el BGM el puerto de descarga es CRMOB = Costa Rica - Moín
        public string LocationCode { get; set; }
        //  Definido por mutuo acuerdo = ZZZ
        public string CodeListIdentification { get; set; }
        // Aduanas de Costa Rica = 98
        public string CodeListResponsibleAgency { get; set; }
        // Nombre del puerto de salida = Guayaquil
        public string LocationName { get; set; }
        // Si el Puerto es costarricense, se debe incluir el Código de 4 dígitos IMO-GISIS = 0002
        public string CountryNameCode { get; set; }
        public BGM BGM { get; set; }


        public override string ToEDIString()
        {

            if (BGM != null && BGM.DocumentName.Contains("119"))
            {

                return $"LOC+{(string.IsNullOrEmpty(LocationQualifier) ? "41" : LocationQualifier)}+{(string.IsNullOrEmpty(LocationCode) ? "0002" : LocationCode)}'";
            }

            else if (BGM != null && BGM.DocumentName.Contains("122"))
            {

                return $"LOC+{(string.IsNullOrEmpty(LocationQualifier) ? "42" : LocationQualifier)}+{(string.IsNullOrEmpty(LocationCode) ? "0002" : LocationCode)}'";
            }
            else
            {
                return string.Empty;
            }
        }

        public override string ToCustomEDI()
        {
            if (BGM != null && BGM.DocumentName.Contains("122"))
            {
                if ((LocationCode.Equals("CRMOB") && LocationQualifier.Equals("9")) || (LocationCode.Equals("CRMOB") && LocationQualifier.Equals("61")))
                {
                    // Loading report
                    return $"LOC+{(string.IsNullOrEmpty(LocationQualifier) ? "9" : LocationQualifier)}+{(string.IsNullOrEmpty(LocationCode) ? "" : LocationCode)}:{(string.IsNullOrEmpty(CodeListIdentification) ? "ZZZ" : CodeListIdentification)}:{(string.IsNullOrEmpty(CodeListResponsibleAgency) ? "98" : CodeListResponsibleAgency)}:{(string.IsNullOrEmpty(LocationName) ? "" : LocationName)}+{(string.IsNullOrEmpty(CountryNameCode) ? "0002" : CountryNameCode)}'";
                }
                else if ((LocationCode.Equals("CRCLIO") && LocationQualifier.Equals("9")) || (LocationCode.Equals("CRCLIO") && LocationQualifier.Equals("61")))
                {
                    return $"LOC+{(string.IsNullOrEmpty(LocationQualifier) ? "9" : LocationQualifier)}+{(string.IsNullOrEmpty(LocationCode) ? "" : LocationCode)}:{(string.IsNullOrEmpty(CodeListIdentification) ? "ZZZ" : CodeListIdentification)}:{(string.IsNullOrEmpty(CodeListResponsibleAgency) ? "98" : CodeListResponsibleAgency)}:{(string.IsNullOrEmpty(LocationName) ? "" : LocationName)}+{(string.IsNullOrEmpty(CountryNameCode) ? "0001" : CountryNameCode)}'";
                }
                else if (LocationQualifier.Equals("94") && (!LocationCode.Equals("CRMOB") && !LocationCode.Equals("CRCLIO")))
                {
                    return $"LOC+{(string.IsNullOrEmpty(LocationQualifier) ? "94" : LocationQualifier)}+{(string.IsNullOrEmpty(LocationCode) ? "" : LocationCode)}:{(string.IsNullOrEmpty(CodeListIdentification) ? "ZZZ" : CodeListIdentification)}:{(string.IsNullOrEmpty(CodeListResponsibleAgency) ? "98" : CodeListResponsibleAgency)}:{(string.IsNullOrEmpty(LocationName) ? "" : LocationName)}'";
                }
                else
                {
                    return $"LOC+{(string.IsNullOrEmpty(LocationQualifier) ? "61" : LocationQualifier)}+{(string.IsNullOrEmpty(LocationCode) ? "" : LocationCode)}:{(string.IsNullOrEmpty(CodeListIdentification) ? "ZZZ" : CodeListIdentification)}:{(string.IsNullOrEmpty(CodeListResponsibleAgency) ? "98" : CodeListResponsibleAgency)}:{(string.IsNullOrEmpty(LocationName) ? "" : LocationName)}'";
                }
            }
            else if (BGM != null && BGM.DocumentName.Contains("119"))
            {
                // Loading report
                return $"LOC+{(string.IsNullOrEmpty(LocationQualifier) ? "11" : LocationQualifier)}+{(string.IsNullOrEmpty(LocationCode) ? "" : LocationCode)}:{(string.IsNullOrEmpty(CodeListIdentification) ? "ZZZ" : CodeListIdentification)}:{(string.IsNullOrEmpty(CodeListResponsibleAgency) ? "98" : CodeListResponsibleAgency)}:{(string.IsNullOrEmpty(LocationName) ? "" : LocationName)}+{(string.IsNullOrEmpty(CountryNameCode) ? "0002" : CountryNameCode)}'";
            }
            else
            {
                // Discharge report
                return $"LOC+{(string.IsNullOrEmpty(LocationQualifier) ? "9" : LocationQualifier)}+{(string.IsNullOrEmpty(LocationCode) ? "" : LocationCode)}:{(string.IsNullOrEmpty(CodeListIdentification) ? "ZZZ" : CodeListIdentification)}:{(string.IsNullOrEmpty(CodeListResponsibleAgency) ? "98" : CodeListResponsibleAgency)}:{(string.IsNullOrEmpty(LocationName) ? "" : LocationName)}'";
            }
        }

        public string Location(string LocationQualifier, string LocationIdentifier, string CodeListIdentification, string CodeListResponsibleAgency)
        {
            if (LocationQualifier.Equals("147"))
            {
                return $"LOC+{(string.IsNullOrEmpty(LocationQualifier) ? "147" : LocationQualifier)}+{LocationIdentifier}:{CodeListIdentification}:{CodeListResponsibleAgency}'";
            }
            else if (LocationQualifier.Equals("9"))
            {
                return $"LOC+{(string.IsNullOrEmpty(LocationQualifier) ? "9" : LocationQualifier)}+{LocationIdentifier}'";
            }
            else
            {
                return $"LOC+{(string.IsNullOrEmpty(LocationQualifier) ? "11" : LocationQualifier)}+{LocationIdentifier}'";
            }
        }
    }
}