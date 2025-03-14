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
            
            else if (BGM != null && BGM.DocumentName.Contains("122")){

                return $"LOC+{(string.IsNullOrEmpty(LocationQualifier) ? "42" : LocationQualifier)}+{(string.IsNullOrEmpty(LocationCode) ? "0002" : LocationCode)}'";
            }
            else
            {
                return string.Empty;
            }
        }
        
        public override string ToCustomEDI()
        {
            if (BGM != null && BGM.DocumentName.Contains("122")){
                // Loading report
                return $"LOC+{(string.IsNullOrEmpty(LocationQualifier) ? "9" : LocationQualifier)}+{(string.IsNullOrEmpty(LocationCode) ? "" : LocationCode)}:{(string.IsNullOrEmpty(CodeListIdentification) ? "ZZZ" : CodeListIdentification)}:{(string.IsNullOrEmpty(CodeListResponsibleAgency) ? "98" : CodeListResponsibleAgency)}:{(string.IsNullOrEmpty(LocationName) ? "" : LocationName)}+{(string.IsNullOrEmpty(CountryNameCode) ? "0002" : CountryNameCode)}'";
            }
            else if (BGM != null && BGM.DocumentName.Contains("119")){
                // Loading report
                return $"LOC+{(string.IsNullOrEmpty(LocationQualifier) ? "11" : LocationQualifier)}+{(string.IsNullOrEmpty(LocationCode) ? "" : LocationCode)}:{(string.IsNullOrEmpty(CodeListIdentification) ? "ZZZ" : CodeListIdentification)}:{(string.IsNullOrEmpty(CodeListResponsibleAgency) ? "98" : CodeListResponsibleAgency)}:{(string.IsNullOrEmpty(LocationName) ? "" : LocationName)}+{(string.IsNullOrEmpty(CountryNameCode) ? "0002" : CountryNameCode)}'";
            }
            else
            {
                // Discharge report
                return $"ELSELOC+{(string.IsNullOrEmpty(LocationQualifier) ? "9" : LocationQualifier)}+{(string.IsNullOrEmpty(LocationCode) ? "" : LocationCode)}:{(string.IsNullOrEmpty(CodeListIdentification) ? "ZZZ" : CodeListIdentification)}:{(string.IsNullOrEmpty(CodeListResponsibleAgency) ? "98" : CodeListResponsibleAgency)}:{(string.IsNullOrEmpty(LocationName) ? "" : LocationName)}'";
            }
        }
    }
}