using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ReadEDIFACT
{
    public class Parser
    {
        public string subject = string.Empty;
        private string[] _lines = Array.Empty<string>();
        private char? segmentSeparator = '\'';
        private char? elementSeparator = '+';
        private char? dataElementSeparator = ':';
        
        public string[] Lines {
            get
            {
                if(this._lines == null)
                {
                    _lines = GetLines();
                }

                return _lines;
            }
        }

        public string[][] ToArray()
        {
            List<string[]> result = new List<string[]>();

            foreach (var line in Lines)
            {
                string[] fields = GetFields(line);

                result.Add(fields);
            }

            return result.ToArray();
        }
        
        public Parser(string subject)
        {
            this.subject = subject;
        }

        public Parser(StreamReader reader)
        {
            using (reader)
            {
                subject = reader.ReadToEnd();
            }            
        }

        
        public string ToCSV()
        {
            string[][] edi = ToArray();
            string result = string.Empty;
            for (int i = 0; i < edi.Length; i++)
            {
                string line = string.Empty;
                for (int j = 0; j < edi[i].Length; j++)
                {
                    line += edi[i][j] + ";";
                }

                result += line + Environment.NewLine;
            }

            return result;
        }

       


        private string[] GetLines()
        {
            List<string> lines = new List<string>();

            if (!string.IsNullOrEmpty(subject))
            {
                int pointer = 0;
                string line = string.Empty;

                while (pointer < subject.Length)
                {
                    if (subject[pointer].Equals('\''))
                    {
                        lines.Add(line);
                        line = string.Empty;
                    }
                    else if (subject[pointer].Equals('?'))
                    {
                        line += subject[++pointer];                        
                    }
                    else if (!subject[pointer].Equals(System.Environment.NewLine))
                    {
                        line += subject[pointer];
                    }

                    pointer++;
                }
            }

            return lines.ToArray();
        }

        private string[] GetFields(string line)
        {
            int pointer = 0;
            List<string> fields = new List<string>();
            string field = string.Empty;
                        
            while(pointer < line.Length)
            {
                if (line[pointer].Equals('+'))
                {
                    fields.Add(field);
                    field = string.Empty;
                }
                else if (line[pointer].Equals('\\'))
                {
                    field += line[++pointer];
                }
                else if (!line[pointer].Equals(Environment.NewLine))
                {
                    field += line[pointer];
                }

                pointer++;
            }

            fields.Add(field);

            return fields.ToArray();
        }        
    }
}