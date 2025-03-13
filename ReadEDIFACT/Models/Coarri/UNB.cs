using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReadEDIFACT.Models.Coarri
{
    public class UNB :EDISegment
    {
        public string SyntaxIdentifier { get; set; }
        public string SyntaxVersion {get; set;}
        public string SenderIdentification { get; set; }
        public string ReceiverIdentification { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
        public string InterchangeRef { get; set; }
        private static int _counter = 0;

        public override string ToEDIString()
        {

            return $"UNB+{(string.IsNullOrEmpty(SyntaxIdentifier) ? "UNOA" : SyntaxIdentifier)}:{(string.IsNullOrEmpty(SyntaxVersion) ? "2" : SyntaxVersion)}+{(string.IsNullOrEmpty(SenderIdentification) ? "JAPDEVACRMOB" : SenderIdentification)}+{(string.IsNullOrEmpty(ReceiverIdentification) ? "CRCUSTOMS01" : ReceiverIdentification)}+{(string.IsNullOrEmpty(Date) ? DateTime.UtcNow.ToString("yyyyMMdd") : Date)}:{(string.IsNullOrEmpty(Time) ? DateTime.UtcNow.ToString("HHmm") : Time )}+{GenerateInterchangeRef()}'";
        }

        public override string ToCustomEDI()
        {
            return $"";
        }


        // Método para generar un número único de referencia
        public static string GenerateInterchangeRef()
        {
            string timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmss");

            if (timestamp.Length > 10)
            {
                timestamp = timestamp.Substring(timestamp.Length - 10);
            }

            Random random = new Random();
            // Número entre 1000 y 9999
            string randomNumber = random.Next(1000, 9999).ToString(); 

            
            string interchangeRef = $"{timestamp}{randomNumber}";

           
            if (interchangeRef.Length > 14)
            {
                interchangeRef = interchangeRef.Substring(0, 14);
            }

            return interchangeRef;
        }
    }
}