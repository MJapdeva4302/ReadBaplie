using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace ReadEDIFACT.Models
{
    public class ParserEDI
    {
        public string Subject { get; private set; }
        private char SegmentSeparator { get; set; }
        private char ElementSeparator { get; set; }
        private char DataElementSeparator { get; set; }
        private FileDefinition Definition { get; set; }

        public ParserEDI(string subject, FileDefinition definition)
        {
            Subject = subject;
            Definition = definition;
            SegmentSeparator = definition.SegmentSeparator;
            ElementSeparator = definition.ElementSeparator;
            DataElementSeparator = definition.DataElementSeparator;
        }

        public ParserEDI(StreamReader reader, FileDefinition definition)
        {
            using (reader)
            {
                Subject = reader.ReadToEnd();
            }
            Definition = definition;
            SegmentSeparator = definition.SegmentSeparator;
            ElementSeparator = definition.ElementSeparator;
            DataElementSeparator = definition.DataElementSeparator;
        }

        public List<string> Validate()
        {
            var errors = new List<string>();
            var lines = Subject.Split(SegmentSeparator);

            for (int lineIndex = 0; lineIndex < lines.Length; lineIndex++)
            {
                var line = lines[lineIndex];
                if (string.IsNullOrWhiteSpace(line)) continue;

                var elements = line.Split(ElementSeparator);
                var segmentId = elements[0];
                var segmentDefinition = FindSegmentDefinition(segmentId, Definition.Segments);

                if (segmentDefinition == null)
                {
                    // Ignorar segmentos no definidos
                    continue;
                }

                if (segmentDefinition is SegmentData segmentData)
                {
                    var segmentErrors = ValidateSegmentData(segmentData, elements, lineIndex);
                    errors.AddRange(segmentErrors);
                }
                else if (segmentDefinition is SegmentGroup segmentGroup)
                {
                    var groupErrors = ValidateSegmentGroup(segmentGroup, elements, lineIndex);
                    errors.AddRange(groupErrors);
                }
            }

            return errors;
        }

        public List<Dictionary<string, object>> Parse()
        {
            var ediData = new List<Dictionary<string, object>>();
            var lines = Subject.Split(SegmentSeparator);

            for (int lineIndex = 0; lineIndex < lines.Length; lineIndex++)
            {
                var line = lines[lineIndex];
                if (string.IsNullOrWhiteSpace(line)) continue;

                var segment = new Dictionary<string, object>();
                var elements = line.Split(ElementSeparator);
                var segmentId = elements[0];
                segment["SegmentID"] = segmentId;

                var segmentDefinition = FindSegmentDefinition(segmentId, Definition.Segments);
                if (segmentDefinition == null)
                {
                    // Ignorar segmentos no definidos
                    continue;
                }

                if (segmentDefinition is SegmentData segmentData)
                {
                    ProcessSegmentData(segment, elements, segmentData);
                }
                else if (segmentDefinition is SegmentGroup segmentGroup)
                {
                    ProcessSegmentGroup(segment, elements, segmentGroup);
                }

                ediData.Add(segment);
            }

            return ediData;
        }

        private List<string> ValidateSegmentData(SegmentData segmentData, string[] elements, int lineIndex)
        {
            var errors = new List<string>();

            int elementIndex = 1; // Empezar desde el segundo elemento (el primero es el SegmentID)
            for (int i = 0; i < segmentData.DataElements.Count(); i++)
            {
                var element = segmentData.DataElements.ElementAt(i);

                if (element is DataElement dataElement)
                {
                    var value = elementIndex < elements.Length ? elements[elementIndex] : null;
                    var elementErrors = ValidateDataElement(dataElement, value, lineIndex);
                    errors.AddRange(elementErrors);
                    elementIndex++;
                }
                else if (element is CompositeElement compositeElement)
                {
                    var value = elementIndex < elements.Length ? elements[elementIndex] : null;
                    var compositeErrors = ValidateCompositeElement(compositeElement, value, lineIndex);
                    errors.AddRange(compositeErrors);
                    elementIndex++;
                }
                else if (element is EmptyElement)
                {
                    // Ignorar EmptyElement (no se valida)
                    elementIndex++;
                }
            }

            return errors;
        }

        private List<string> ValidateCompositeElement(CompositeElement compositeElement, string value, int lineIndex)
        {
            var errors = new List<string>();

            if (string.IsNullOrEmpty(value))
            {
                if (compositeElement.Usage == RuleUsage.Mandatory)
                {
                    errors.Add($"El elemento compuesto '{compositeElement.Name}' es obligatorio y no se proporcionó (Línea {lineIndex + 1}).");
                }
                return errors;
            }

            // Dividir el valor usando el DataElementSeparator
            var subElements = value.Split(DataElementSeparator);
            for (int i = 0; i < compositeElement.DataElements.Count(); i++)
            {
                var dataElement = compositeElement.DataElements.ElementAt(i);
                var subValue = i < subElements.Length ? subElements[i] : null;

                var elementErrors = ValidateDataElement((DataElement)dataElement, subValue, lineIndex);
                errors.AddRange(elementErrors);
            }

            return errors;
        }

        private List<string> ValidateSegmentGroup(SegmentGroup segmentGroup, string[] elements, int lineIndex)
        {
            var errors = new List<string>();

            foreach (var groupSegment in segmentGroup.Segments)
            {
                if (groupSegment is SegmentData groupSegmentData)
                {
                    var segmentErrors = ValidateSegmentData(groupSegmentData, elements, lineIndex);
                    errors.AddRange(segmentErrors);
                }
                else if (groupSegment is SegmentGroup nestedSegmentGroup)
                {
                    var nestedErrors = ValidateSegmentGroup(nestedSegmentGroup, elements, lineIndex);
                    errors.AddRange(nestedErrors);
                }
            }

            return errors;
        }

        private List<string> ValidateDataElement(DataElement dataElement, string value, int lineIndex)
        {
            var errors = new List<string>();
            // var data = $"VALUE: {value}  ---  Precision: {dataElement.Precision}  ---  DataType:  {dataElement.DataType}";
            // File.WriteAllText(@"C:\Users\mbermudez\Documents\ReadBaplie\ReadEDIFACT\Output.txt", data);
            // Console.WriteLine($"VALUE: {value}  ---  Precision: {dataElement.Precision}  ---  DataType:  {dataElement.DataType}");
            
            if (string.IsNullOrEmpty(value))
            {
                if (dataElement.Usage == RuleUsage.Mandatory)
                {
                    errors.Add($"El elemento '{dataElement.Name}' es obligatorio y no se proporcionó (Línea {lineIndex + 1}).");
                }
                return errors;
            }

            if (!IsValidLength(value, dataElement.Precision, dataElement.DataType))
            {
                // Console.WriteLine($"VALUE: {value}  ---  Precision: {dataElement.Precision}  ---  DataType:  {dataElement.DataType}");
                errors.Add($"El valor '{value}' no tiene la longitud válida para el elemento '{dataElement.Name}' (Línea {lineIndex + 1}).");
            }


            if (!IsValidDataType(value, dataElement.DataType))
            {
                // Console.WriteLine($"VALUE: {value}  ---  DataType:  {dataElement.DataType}");
                errors.Add($"El valor '{value}' no es válido para el elemento '{dataElement.Name}' (Línea {lineIndex + 1}).");
            }

            return errors;
        }

        private bool IsValidLength(string value, object precision, DataType dataType)
        {

            if (dataType == DataType.Decimal)
            {

                int digitLength = value.Replace(".", "").Replace("-", "").Length;

                if (precision is int length)
                {
                    // Console.WriteLine($"{digitLength} ---- {length}");
                    return digitLength == length;
                }
                else if (precision is int[] range && range.Length == 2)
                {

                    int minLength = range[0];
                    int maxLength = range[1];
                    return digitLength >= minLength && digitLength <= maxLength;
                }
            }
            else
            {

                if (precision is int length)
                {
                    return value.Length == length;
                }
                else if (precision is int[] range && range.Length == 2)
                {
                    int minLength = range[0];
                    int maxLength = range[1];
                    return value.Length >= minLength && value.Length <= maxLength;
                }
            }

            return true;
        }

        private bool IsValidDataType(string value, DataType dataType)
        {
            switch (dataType)
            {
                case DataType.Alphabetic:
                    return value.All(c => char.IsLetter(c) || c == ' ');
                case DataType.Alphanumeric:
                    return value.All(c => char.IsLetterOrDigit(c) || c == ' ' || c == '/' || c == '-' || c == '(' || c == ')' || c == '*');
                case DataType.Numeric:
                    return value.All(char.IsDigit);
                case DataType.Decimal:

                    bool hasDecimalPoint = false;
                    bool hasNegativeSign = false;

                    for (int i = 0; i < value.Length; i++)
                    {
                        char c = value[i];

                        if (c == '-')
                        {

                            if (i != 0 || hasNegativeSign)
                            {
                                return false;
                            }
                            hasNegativeSign = true;
                        }
                        else if (c == '.')
                        {

                            if (hasDecimalPoint)
                            {
                                return false;
                            }
                            hasDecimalPoint = true;
                        }
                        else if (!char.IsDigit(c))
                        {
                            return false;
                        }
                    }
                    return true;
                case DataType.Other:
                default:
                    return true;
            }
        }

        private Segment FindSegmentDefinition(string segmentId, IEnumerable<Segment> segments)
        {
            foreach (var segment in segments)
            {
                if (segment is SegmentData segmentData && segmentData.SegmentID == segmentId)
                {
                    //Console.WriteLine($"FindSegmentDefinition 2:  {segment}");
                    return segmentData;
                }
                else if (segment is SegmentGroup segmentGroup)
                {
                    //Console.WriteLine($"FindSegmentDefinition 2: {segment}  --- {segmentGroup}");
                    var nestedSegment = FindSegmentDefinition(segmentId, segmentGroup.Segments);
                    if (nestedSegment != null)
                    {
                        return nestedSegment;
                    }
                }
            }

            return null;
        }

        private void ProcessSegmentData(Dictionary<string, object> segment, string[] elements, SegmentData segmentData)
        {
            for (int i = 1; i < elements.Length; i++)
            {
                var elementDefinition = segmentData.DataElements.ElementAtOrDefault(i - 1);
                if (elementDefinition == null) continue;

                if (elementDefinition is CompositeElement compositeElement)
                {
                    var compositeData = new Dictionary<string, object>();
                    var subElements = elements[i].Split(DataElementSeparator);

                    for (int j = 0; j < subElements.Length; j++)
                    {
                        var subElement = compositeElement.DataElements.ElementAtOrDefault(j);
                        if (subElement == null) continue;

                        if (subElement is DataElement subElementDefinition)
                        {
                            string value = subElements[j];

                            // Parsear fechas y horas si el nombre del campo lo indica
                            if (subElementDefinition.Name == "Date of preparation" ||
                                subElementDefinition.Name == "Time of preparation")
                            {
                                string format = GetDateTimeFormat(subElementDefinition.Name); // Obtener el formato
                                value = DateTimeParser.ParseDateTime(value, format);
                            }

                            compositeData[subElementDefinition.Name] = value;
                        }
                    }

                    segment[elementDefinition.Name] = compositeData;
                }
                else if (elementDefinition is DataElement dataElement)
                {
                    string value = elements[i];

                    // Parsear fechas y horas si el nombre del campo lo indica
                    if (dataElement.Name == "Date of preparation" ||
                        dataElement.Name == "Time of preparation")
                    {
                        string format = GetDateTimeFormat(dataElement.Name); // Obtener el formato
                        value = DateTimeParser.ParseDateTime(value, format);
                    }

                    segment[dataElement.Name] = value;
                }
            }
        }

        private void ProcessSegmentGroup(Dictionary<string, object> segment, string[] elements, SegmentGroup segmentGroup)
        {
            var groupData = new List<Dictionary<string, object>>();

            foreach (var groupSegment in segmentGroup.Segments)
            {
                if (groupSegment is SegmentData groupSegmentData)
                {
                    var groupSegmentDict = new Dictionary<string, object>();
                    ProcessSegmentData(groupSegmentDict, elements, groupSegmentData);
                    groupData.Add(groupSegmentDict);
                }
                else if (groupSegment is SegmentGroup nestedSegmentGroup)
                {
                    var nestedGroupDict = new Dictionary<string, object>();
                    ProcessSegmentGroup(nestedGroupDict, elements, nestedSegmentGroup);
                    groupData.Add(nestedGroupDict);
                }
            }

            segment[segmentGroup.Name] = groupData;
        }

        private string GetDateTimeFormat(string elementName)
        {
            // Define el formato según el nombre del campo
            switch (elementName)
            {
                case "Date of preparation":
                    return "101"; // YYMMDD
                case "Time of preparation":
                    return "201"; // YYMMDDHHMM
                default:
                    return ""; // Sin formato específico
            }
        }

        public string ToJson()
        {
            var parsedData = Parse();
            return JsonConvert.SerializeObject(parsedData, Formatting.Indented);
        }

        public void SaveJsonToFile(string filePath)
        {
            string json = ToJson(); 
            File.WriteAllText(filePath, json); 
        }
    }
}