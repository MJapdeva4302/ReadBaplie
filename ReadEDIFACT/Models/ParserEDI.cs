using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Mail;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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

        // Método para validar el tipo de mensaje (BAPLIE, MOVINS, COARRI, ECT)
        public List<string> ValidateMessageType(string name)
        {
            var errors = new List<string>();

            // Buscar el segmento UNH
            var unhSegment = Subject.Split(SegmentSeparator)
                                   .FirstOrDefault(line => line.StartsWith("UNH"));

            if (unhSegment == null)
            {
                errors.Add("No se encontró el segmento UNH en el archivo EDI.");
                return errors;
            }

            // Extraer los elementos del segmento UNH
            var elements = unhSegment.Split(ElementSeparator); // ElementSeparator es '+'
            if (elements.Length < 3) // Cambia a 3 porque necesitamos al menos 3 elementos
            {
                errors.Add("El segmento UNH no tiene suficientes elementos.");
                return errors;
            }

            // Imprimir los elementos del segmento UNH
            // Console.WriteLine($"Elementos del segmento UNH:");
            // for (int i = 0; i < elements.Length; i++)
            // {
            //     Console.WriteLine($"Elemento {i}: {elements[i]}");
            // }

            var messageIdentifier = elements[2];

            var messageIdentifierParts = messageIdentifier.Split(DataElementSeparator);
            if (messageIdentifierParts.Length < 1)
            {
                errors.Add("El Message Identifier no tiene el formato esperado.");
                return errors;
            }

            // Imprimir las partes del Message Identifier
            // Console.WriteLine($"Partes del Message Identifier:");
            // for (int i = 0; i < messageIdentifierParts.Length; i++)
            // {
            //     Console.WriteLine($"Parte {i}: {messageIdentifierParts[i]}");
            // }

            // EXTRAE LA PRIMERA PARTE PARA COMPARAR SI ES EL MISMO NAME QUE ESTA DEFINIDO EN MI FILEDEFINITION
            var messageTypeIdentifier = messageIdentifierParts[0];

            // COMPARAR CON EL NAME DEL FILEDEFINITION CON EL QUE TIENE EL ARCHIVO EDI
            if (messageTypeIdentifier != name)
            {
                errors.Add($"El archivo no es un {name}. Tipo de mensaje encontrado: {messageTypeIdentifier}");
            }

            return errors;
        }


        public List<string> ValidateFullEDI(string name)
        {
            var errors = new List<string>();

            // Valida el tipo de mensaje (BAPLIE)
            var messageTypeErrors = ValidateMessageType(name);
            errors.AddRange(messageTypeErrors);

            if (errors.Any())
            {
                // EN CASO DE ERRORES FINALIZAR Y MOSTRAR LOS ERRORES
                return errors;
            }

            // Valida la estructura del archivo
            var structureErrors = Validate();
            errors.AddRange(structureErrors);

            return errors;
        }

        public List<string> Validate()
        {
            var errors = new List<string>();
            // Divide el archivo EDI en segmentos
            var lines = Subject.Split(SegmentSeparator);

            for (int lineIndex = 0; lineIndex < lines.Length; lineIndex++)
            {
                var line = lines[lineIndex];
                if (string.IsNullOrWhiteSpace(line)) continue;

                // Divide el segmento en elementos
                var elements = line.Split(ElementSeparator);

                var segmentId = elements[0];


                var segmentDefinition = FindSegmentDefinition(segmentId, elements, Definition.Segments);

                if (segmentDefinition == null)
                {

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

                var segmentDefinition = FindSegmentDefinition(segmentId, elements, Definition.Segments);
                if (segmentDefinition == null)
                {

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
            // if (!string.IsNullOrEmpty(segmentData.Notes))
            // {
            //     elements["Notes"] = segmentData.Notes;
            // }
            int elementIndex = 1;
            for (int i = 0; i < segmentData?.DataElements?.Count(); i++)
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
                    var compositeErrors = ValidateCompositeElement(compositeElement, value ?? "", lineIndex);
                    errors.AddRange(compositeErrors);
                    elementIndex++;
                }
                else if (element is EmptyElement)
                {
                    // Ignorar EmptyElement (no se valida)
                    elementIndex++;
                }
            }

            // Validar si hay más elementos en el segmento que no están definidos en las reglas
            if (elementIndex < elements.Length)
            {
                for (int i = elementIndex; i < elements.Length; i++)
                {
                    errors.Add($"El elemento en la posición {i + 1} del segmento '{segmentData?.SegmentID}' en la línea {lineIndex + 1} no está definido en las reglas de validación.");
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
            for (int i = 0; i < compositeElement?.DataElements?.Count(); i++)
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

            if (string.IsNullOrEmpty(value))
            {
                if (dataElement.Usage == RuleUsage.Mandatory)
                {
                    errors.Add($"El elemento '{dataElement.Name}' es obligatorio y no se proporcionó (Línea {lineIndex + 1}). Desription: {dataElement.Description} --- {dataElement.Precision} --- {dataElement.DataType}");
                }
                return errors;
            }

            if (!IsValidLength(value, dataElement.Precision, dataElement.DataType))
            {
                errors.Add($"El valor '{value}' no tiene la longitud válida para el elemento '{dataElement.Name}' (Línea {lineIndex + 1}).");
            }

            if (!IsValidDataType(value, dataElement.DataType))
            {
                errors.Add($"El valor '{value}' no es válido para el elemento '{dataElement.Name}' (Línea {lineIndex + 1}).");
            }

            return errors;
        }

        private bool IsSegmentStructureValid(SegmentData segmentData, string[] ediElements)
        {

            int ediElementIndex = 1;
            int ruleElementIndex = 0;


            while (ruleElementIndex < segmentData.DataElements.Count() && ediElementIndex < ediElements.Length)
            {
                var ruleElement = segmentData.DataElements.ElementAt(ruleElementIndex);

                if (ruleElement is DataElement dataElement)
                {

                    if (dataElement.Usage == RuleUsage.Mandatory && string.IsNullOrEmpty(ediElements[ediElementIndex]))
                    {

                        return false;
                    }


                    ediElementIndex++;
                    ruleElementIndex++;
                }
                else if (ruleElement is CompositeElement compositeElement)
                {

                    var compositeValues = ediElements[ediElementIndex].Split(DataElementSeparator);
                    int compositeValueIndex = 0;

                    foreach (var subElement in compositeElement.DataElements)
                    {
                        if (subElement is DataElement subDataElement)
                        {
                            if (subDataElement.Usage == RuleUsage.Mandatory &&
                                (compositeValueIndex >= compositeValues.Length || string.IsNullOrEmpty(compositeValues[compositeValueIndex])))
                            {

                                return false;
                            }

                            compositeValueIndex++;
                        }
                    }

                    ediElementIndex++;
                    ruleElementIndex++;
                }
                else if (ruleElement is EmptyElement)
                {

                    ediElementIndex++;
                    ruleElementIndex++;
                }
            }


            while (ruleElementIndex < segmentData.DataElements.Count())
            {
                var ruleElement = segmentData.DataElements.ElementAt(ruleElementIndex);

                if (ruleElement is DataElement dataElement && dataElement.Usage == RuleUsage.Mandatory)
                {

                    return false;
                }

                ruleElementIndex++;
            }


            return true;
        }

        private bool IsValidLength(string value, object precision, DataType dataType)
        {
            if (dataType == DataType.Decimal)
            {
                int digitLength = value.Replace(".", "").Replace("-", "").Length;

                if (precision is int length)
                {
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

        private Segment FindSegmentDefinition(string segmentId, string[] ediElements, IEnumerable<Segment> segments)
        {
            Console.WriteLine($"SegmentID: {segmentId} --- EdiElements: {ediElements} --- Segments: {segments}");
            foreach (var segment in segments)
            {
                if (segment is SegmentData segmentData && segmentData.SegmentID == segmentId)
                {

                    if (IsSegmentStructureValid(segmentData, ediElements))
                    {
                        return segmentData;
                    }
                }
                else if (segment is SegmentGroup segmentGroup)
                {

                    var nestedSegment = FindSegmentDefinition(segmentId, ediElements, segmentGroup.Segments);
                    if (nestedSegment != null)
                    {
                        return nestedSegment;
                    }
                }
            }

            // throw new InvalidOperationException("No segment definition found for the given segment ID.");
            return null;
        }

        private void ProcessSegmentData(Dictionary<string, object> segment, string[] elements, SegmentData segmentData)
        {

            if (!string.IsNullOrEmpty(segmentData.Notes))
            {
                segment["Notes"] = segmentData.Notes;
            }


            int elementIndex = 1;


            foreach (var elementDefinition in segmentData.DataElements)
            {
                if (elementDefinition is CompositeElement compositeElement)
                {

                    var compositeData = new Dictionary<string, object>();
                    string compositeValue = elementIndex < elements.Length ? elements[elementIndex] : "";

                    if (!string.IsNullOrEmpty(compositeValue))
                    {
                        var subElements = compositeValue.Split(DataElementSeparator);

                        for (int j = 0; j < subElements.Length; j++)
                        {
                            var subElement = compositeElement?.DataElements?.ElementAtOrDefault(j);
                            if (subElement == null) continue;

                            if (subElement is DataElement subElementDefinition)
                            {
                                string value = subElements[j];
                                compositeData[subElementDefinition.Name] = value;
                            }
                        }
                    }

                    segment[elementDefinition.Name ?? ""] = compositeData;
                    elementIndex++;
                }
                else if (elementDefinition is DataElement dataElement)
                {

                    string value = elementIndex < elements.Length ? elements[elementIndex] : "";
                    segment[dataElement.Name ?? ""] = value;
                    elementIndex++;
                }
                else if (elementDefinition is EmptyElement)
                {

                    segment[$"Empty{elementIndex}"] = "";
                    elementIndex++;
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

        /*private string GetDateTimeFormat(string elementName)
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
        }*/

        public string ToJson()
        {
            var parsedData = Parse();
            var jsonObject = new Dictionary<string, object>
            {
                { "Name", Definition.Name },
                { "Version", Definition.Version?.ToString() ?? "Unknown" }
            };

            jsonObject.Add("ParsedData", parsedData);
            return JsonConvert.SerializeObject(jsonObject, Formatting.Indented);
        }
        // public void ToSeeJson()
        // {
        //     var parsedData = Parse();
        //     var json = JsonConvert.SerializeObject(parsedData, Formatting.Indented);
        //     Console.WriteLine($"ToJson:: {json}");
        //     // return JsonConvert.SerializeObject(parsedData, Formatting.Indented);
        // }

        public void SaveJsonToFile(string filePath)
        {
            string json = ToJson();
            File.WriteAllText(filePath, json);
        }

        // ACA VAMOS A IMPLEMENTAR LOS METODOS PARA PASAR DE JSON A EDI
        public string GenerateEDIFromJson(string jsonContent)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(jsonContent))
                {
                    throw new ArgumentException("El contenido del JSON no puede estar vacío.");
                }

                // Parsear el JSON como un objeto (no como un array)
                var jsonObject = JObject.Parse(jsonContent);

                // Extraer el array "ParsedData"
                var parsedData = jsonObject["ParsedData"] as JArray;
                if (parsedData == null)
                {
                    throw new ArgumentException("El JSON no contiene un array 'ParsedData' válido.");
                }

                var ediSegments = new List<string>();

                // Procesar cada segmento dentro de "ParsedData"
                foreach (var segmentJson in parsedData)
                {
                    var segmentID = segmentJson["SegmentID"]?.ToString();
                    if (string.IsNullOrEmpty(segmentID))
                    {
                        throw new ArgumentException("El SegmentID no puede estar vacío en el JSON.");
                    }

                    var ediSegment = GenerateSegmentFromJson(segmentJson);
                    ediSegments.Add(ediSegment);
                }

                // Unir los segmentos EDI con el separador de segmentos
                return string.Join(Definition.SegmentSeparator.ToString(), ediSegments) + Definition.SegmentSeparator;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al generar el archivo EDI a partir del JSON.", ex);
            }
        }

        private bool IsEmptyValue(JToken value)
        {
            if (value.Type == JTokenType.Object)
            {
                return !value.Children().Any();
            }
            else if (value.Type == JTokenType.Array)
            {
                return !value.Children().Any();
            }
            else
            {
                return string.IsNullOrEmpty(value.ToString());
            }
        }

        private string GenerateSegmentFromJson(JToken segmentJson)
        {
            var elements = new List<string>();

            var segmentID = segmentJson["SegmentID"]?.ToString();
            elements.Add(segmentID);

            foreach (var property in segmentJson.Children<JProperty>())
            {
                if (property.Name != "SegmentID")
                {
                    var value = property.Value;

                    if (property.Name == "location one identification" || property.Name == "location two identification")
                    {
                        if (IsEmptyValue(value))
                        {
                            continue;
                        }
                    }

                    if (property.Name.StartsWith("Empty"))
                    {
                        elements.Add("");
                        continue;
                    }

                    if (value.Type == JTokenType.Object)
                    {
                        var compositeValue = GenerateCompositeValueFromJson(value);
                        if (!string.IsNullOrEmpty(compositeValue))
                        {
                            elements.Add(compositeValue);
                        }
                    }
                    else if (value.Type == JTokenType.Array)
                    {
                        foreach (var item in value)
                        {
                            var itemValue = GenerateCompositeValueFromJson(item);
                            if (!string.IsNullOrEmpty(itemValue))
                            {
                                elements.Add(itemValue);
                            }
                        }
                    }
                    else
                    {
                        elements.Add(value.ToString());
                    }
                }
            }

            return string.Join(Definition.ElementSeparator.ToString(), elements);
        }

        private string GenerateCompositeValueFromJson(JToken compositeJson)
        {
            var subElements = new List<string>();

            foreach (var property in compositeJson.Children<JProperty>())
            {
                var value = property.Value;

                if (string.IsNullOrEmpty(value.ToString()))
                {
                    subElements.Add("");
                }
                else
                {
                    subElements.Add(value.ToString());
                }
            }

            return string.Join(Definition.DataElementSeparator.ToString(), subElements);
        }
    }
}