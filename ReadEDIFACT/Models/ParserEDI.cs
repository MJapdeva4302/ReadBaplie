using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
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

        // Método para validar el tipo de mensaje (BAPLIE)
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

            // Depuración: Imprimir el segmento UNH completo
            // Console.WriteLine($"Segmento UNH completo: {unhSegment}");

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


            // Console.WriteLine($"Message Identifier: {messageIdentifier}");


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

            // Console.WriteLine($"Message Type Identifier: {messageTypeIdentifier}");

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

            // 1. Validar el tipo de mensaje (BAPLIE)
            var messageTypeErrors = ValidateMessageType(name);
            errors.AddRange(messageTypeErrors);

            if (errors.Any())
            {
                return errors; // Si hay errores en el tipo de mensaje, no continuar
            }

            // 2. Validar la estructura del archivo
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
                //Obtiene el primer elemento que en este caso es el segmentID
                var segmentId = elements[0];

                // Busca la definición del segmento en las reglas de validación
                var segmentDefinition = FindSegmentDefinition(segmentId, elements, Definition.Segments);

                if (segmentDefinition == null)
                {
                    // Que continue si no hay segmentos definidos
                    continue;
                }

                // Valida el segmento según su tipo (SegmentData o SegmentGroup)
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

        // Metodo que me devuelve uma lista del archivo edi ya procesada para pasarla despues a json
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
                    // Que continue si no hay segmentos definidos
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

            // Validar si hay más elementos en el segmento que no están definidos en las reglas
            if (elementIndex < elements.Length)
            {
                for (int i = elementIndex; i < elements.Length; i++)
                {
                    errors.Add($"El elemento en la posición {i + 1} del segmento '{segmentData.SegmentID}' en la línea {lineIndex + 1} no está definido en las reglas de validación.");
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
            // El primer elemento es el SegmentID, así que empezamos desde el segundo elemento
            int ediElementIndex = 1;
            int ruleElementIndex = 0;

            // Recorremos los elementos definidos en las reglas de validación
            while (ruleElementIndex < segmentData.DataElements.Count() && ediElementIndex < ediElements.Length)
            {
                var ruleElement = segmentData.DataElements.ElementAt(ruleElementIndex);

                if (ruleElement is DataElement dataElement)
                {
                    // Si el elemento en las reglas es un DataElement, verificamos si el valor en el EDI es válido
                    if (dataElement.Usage == RuleUsage.Mandatory && string.IsNullOrEmpty(ediElements[ediElementIndex]))
                    {
                        // Si el elemento es obligatorio y está vacío en el EDI, la estructura no es válida
                        return false;
                    }

                    // Avanzamos al siguiente elemento en el EDI y en las reglas
                    ediElementIndex++;
                    ruleElementIndex++;
                }
                else if (ruleElement is CompositeElement compositeElement)
                {
                    // Si el elemento en las reglas es un CompositeElement, validamos sus subelementos
                    var compositeValues = ediElements[ediElementIndex].Split(DataElementSeparator);
                    int compositeValueIndex = 0;

                    foreach (var subElement in compositeElement.DataElements)
                    {
                        if (subElement is DataElement subDataElement)
                        {
                            if (subDataElement.Usage == RuleUsage.Mandatory &&
                                (compositeValueIndex >= compositeValues.Length || string.IsNullOrEmpty(compositeValues[compositeValueIndex])))
                            {
                                // Si el subelemento es obligatorio y está vacío, la estructura no es válida
                                return false;
                            }

                            compositeValueIndex++;
                        }
                    }

                    // Avanzamos al siguiente elemento en el EDI y en las reglas
                    ediElementIndex++;
                    ruleElementIndex++;
                }
                else if (ruleElement is EmptyElement)
                {
                    // Si el elemento en las reglas es un EmptyElement, simplemente avanzamos al siguiente elemento en el EDI
                    ediElementIndex++;
                    ruleElementIndex++;
                }
            }

            // Verificamos si todos los elementos obligatorios en las reglas fueron cubiertos por el EDI
            while (ruleElementIndex < segmentData.DataElements.Count())
            {
                var ruleElement = segmentData.DataElements.ElementAt(ruleElementIndex);

                if (ruleElement is DataElement dataElement && dataElement.Usage == RuleUsage.Mandatory)
                {
                    // Si hay un elemento obligatorio en las reglas que no fue cubierto por el EDI, la estructura no es válida
                    return false;
                }

                ruleElementIndex++;
            }

            // Si llegamos aquí, la estructura del segmento es válida
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
            foreach (var segment in segments)
            {
                if (segment is SegmentData segmentData && segmentData.SegmentID == segmentId)
                {
                    // Verificar si la estructura del segmento coincide con el EDI
                    if (IsSegmentStructureValid(segmentData, ediElements))
                    {
                        return segmentData; // Retorna el segmento si coincide el SegmentID y la estructura
                    }
                }
                else if (segment is SegmentGroup segmentGroup)
                {
                    // Busca recursivamente en los segmentos del grupo
                    var nestedSegment = FindSegmentDefinition(segmentId, ediElements, segmentGroup.Segments);
                    if (nestedSegment != null)
                    {
                        return nestedSegment; // Retorna el segmento si se encuentra en el grupo
                    }
                }
            }

            return null; // Si no se encuentra el segmento
        }

        private void ProcessSegmentData(Dictionary<string, object> segment, string[] elements, SegmentData segmentData)
        {
            // Agregar Notes si está definido
            if (!string.IsNullOrEmpty(segmentData.Notes))
            {
                segment["Notes"] = segmentData.Notes;
            }

            // Índice para recorrer los elementos del segmento EDI
            int elementIndex = 1; // Empezar desde el segundo elemento (el primero es el SegmentID)

            // Recorrer las reglas de validación de los elementos del segmento
            foreach (var elementDefinition in segmentData.DataElements)
            {
                if (elementDefinition is CompositeElement compositeElement)
                {
                    // Procesar CompositeElement
                    var compositeData = new Dictionary<string, object>();
                    string compositeValue = elementIndex < elements.Length ? elements[elementIndex] : "";

                    if (!string.IsNullOrEmpty(compositeValue))
                    {
                        var subElements = compositeValue.Split(DataElementSeparator);

                        for (int j = 0; j < subElements.Length; j++)
                        {
                            var subElement = compositeElement.DataElements.ElementAtOrDefault(j);
                            if (subElement == null) continue;

                            if (subElement is DataElement subElementDefinition)
                            {
                                string value = subElements[j];
                                compositeData[subElementDefinition.Name] = value;
                            }
                        }
                    }

                    segment[elementDefinition.Name] = compositeData;
                    elementIndex++;
                }
                else if (elementDefinition is DataElement dataElement)
                {
                    // Procesar DataElement
                    string value = elementIndex < elements.Length ? elements[elementIndex] : "";
                    segment[dataElement.Name] = value;
                    elementIndex++;
                }
                else if (elementDefinition is EmptyElement)
                {
                    // Manejar EmptyElement: agregar campo vacío al JSON
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
            return JsonConvert.SerializeObject(parsedData, Formatting.Indented);
        }

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
                // Verificar si el JSON está vacío o es nulo
                if (string.IsNullOrWhiteSpace(jsonContent))
                {
                    throw new ArgumentException("El contenido del JSON no puede estar vacío.");
                }

                // Parsear el JSON
                JArray jsonArray = JArray.Parse(jsonContent);

                // Lista para almacenar los segmentos EDI generados
                var ediSegments = new List<string>();

                // Recorrer cada segmento en el JSON
                foreach (var segmentJson in jsonArray)
                {
                    var segmentID = segmentJson["SegmentID"]?.ToString();
                    if (string.IsNullOrEmpty(segmentID))
                    {
                        throw new ArgumentException("El SegmentID no puede estar vacío en el JSON.");
                    }

                    // Buscar la regla correspondiente al segmento en FileDefinition
                    var segmentRule = FindMatchingSegmentRule(segmentID, segmentJson, Definition.Segments);
                    Console.WriteLine($"GenerateEDIFromJson {segmentRule}");
                    if (segmentRule != null)
                    {
                        // Generar el segmento EDI
                        var ediSegment = GenerateSegment(segmentRule, segmentJson);
                        ediSegments.Add(ediSegment);
                    }
                    else
                    {
                        throw new Exception($"No se encontró una regla para el segmento {segmentID}.");
                    }
                }

                // Unir todos los segmentos con el separador de segmentos
                return string.Join(Definition.SegmentSeparator.ToString(), ediSegments) + Definition.SegmentSeparator;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al generar el archivo EDI a partir del JSON.", ex);
            }
        }

        private Segment FindMatchingSegmentRule(string segmentID, JToken segmentJson, IEnumerable<Segment> segments)
        {
            Console.WriteLine($"FindMatchingSegmentRule 1: Buscando regla para {segmentID}");

            foreach (var segment in segments)
            {
                if (segment is SegmentData segmentData && segmentData.SegmentID == segmentID)
                {
                    Console.WriteLine($"FindMatchingSegmentRule 2: Regla encontrada para {segmentID}");

                    // Verificar si la estructura del JSON coincide con las reglas del segmento
                    if (MatchesSegmentStructure(segmentData, segmentJson))
                    {
                        Console.WriteLine($"FindMatchingSegmentRule 3: Estructura válida para {segmentID}");
                        return segmentData;
                    }
                    else
                    {
                        Console.WriteLine($"FindMatchingSegmentRule 3: Estructura no válida para {segmentID}");
                    }
                }
                else if (segment is SegmentGroup segmentGroup)
                {
                    // Si el segmento pertenece a un grupo, buscar en los segmentos del grupo
                    var foundSegment = FindMatchingSegmentRule(segmentID, segmentJson, segmentGroup.Segments);
                    if (foundSegment != null)
                    {
                        return foundSegment;
                    }
                }
            }

            Console.WriteLine($"FindMatchingSegmentRule 4: No se encontró una regla para el segmento {segmentID}");
            return null; // Si no se encuentra el segmento
        }

        /*private string NormalizeName(string name)
        {
            // Eliminar espacios, dos puntos y otros caracteres especiales
            return name.Replace(" ", "").Replace(":", "").Replace(".", "").Replace("-", "").ToLower();
        }*/

        private bool MatchesSegmentStructure(SegmentData segmentData, JToken segmentJson)
        {
            Console.WriteLine($"MatchesSegmentStructure 1:  {segmentData.Name} --- {segmentJson}");

            // Verificar si todos los elementos definidos en las reglas están presentes en el JSON
            foreach (var elementRule in segmentData.DataElements)
            {
                if (elementRule is DataElement dataElement)
                {
                    string normalizedFieldName = dataElement.Name;
                    // Console.WriteLine($"MatchesSegmentStructure 2:  {dataElement.Name} --- {segmentJson[normalizedFieldName]}");

                    // Verificar si el campo está presente en el JSON
                    if (segmentJson[normalizedFieldName] == null)
                    {
                        // Console.WriteLine($"Campo faltante: {dataElement.Name} (Normalized: {normalizedFieldName})");
                        return false;
                    }
                }
                else if (elementRule is CompositeElement compositeElement)
                {
                    string normalizedCompositeName = compositeElement.Name;
                    // Console.WriteLine($"MatchesSegmentStructure 2:  {compositeElement.Name} (Normalized: {normalizedCompositeName}) --- {segmentJson[normalizedCompositeName]}");

                    // Verificar si el campo compuesto está presente en el JSON
                    if (segmentJson[normalizedCompositeName] == null)
                    {
                        // Console.WriteLine($"Campo compuesto faltante: {compositeElement.Name} (Normalized: {normalizedCompositeName})");
                        return false;
                    }

                    // Verificar los subelementos del campo compuesto
                    foreach (var subElementRule in compositeElement.DataElements)
                    {
                        string normalizedSubElementName = subElementRule.Name;
                        // Console.WriteLine($"MatchesSegmentStructure 3:  {subElementRule.Name} (Normalized: {normalizedSubElementName}) --- {segmentJson[normalizedCompositeName]?[normalizedSubElementName]}");

                        if (segmentJson[normalizedCompositeName]?[normalizedSubElementName] == null)
                        {
                            // Console.WriteLine($"Subelemento faltante: {subElementRule.Name} (Normalized: {normalizedSubElementName}) en {compositeElement.Name}");
                            return false;
                        }
                    }
                }
            }

            return true;
        }

        private string GenerateSegment(Segment segmentRule, JToken segmentJson)
        {
            if (segmentRule is SegmentData segmentData)
            {
                return GenerateSegmentData(segmentData, segmentJson);
            }
            else if (segmentRule is SegmentGroup segmentGroup)
            {
                return GenerateSegmentGroup(segmentGroup, segmentJson);
            }
            else
            {
                throw new Exception($"Tipo de segmento no soportado: {segmentRule.GetType().Name}");
            }
        }

        private string GenerateSegmentData(SegmentData segmentData, JToken segmentJson)
        {
            var elements = new List<string>();

            // Añadir el ID del segmento como primer elemento
            elements.Add(segmentData.SegmentID);

            // Recorrer los elementos definidos en las reglas del segmento
            foreach (var elementRule in segmentData.DataElements)
            {
                if (elementRule is DataElement dataElement)
                {
                    // Buscar el valor del elemento en el JSON
                    var elementValue = segmentJson[dataElement.Name]?.ToString();
                    elements.Add(elementValue ?? ""); // Si no existe, agregar vacío
                }
                else if (elementRule is CompositeElement compositeElement)
                {
                    // Generar el elemento compuesto
                    var compositeValue = GenerateCompositeElement(compositeElement, segmentJson);
                    elements.Add(compositeValue);
                }
                else if (elementRule is EmptyElement)
                {
                    // Agregar un "+" para los EmptyElement
                    elements.Add("");
                }
            }

            // Unir los elementos con el separador de elementos
            return string.Join(Definition.ElementSeparator.ToString(), elements);
        }

        private string GenerateSegmentGroup(SegmentGroup segmentGroup, JToken segmentJson)
        {
            var groupSegments = new List<string>();

            // Recorrer los segmentos dentro del grupo
            foreach (var segment in segmentGroup.Segments)
            {
                var segmentID = segment.SegmentID;
                var segmentRule = FindMatchingSegmentRule(segmentID, segmentJson, new[] { segment });
                if (segmentRule != null)
                {
                    // Generar el segmento EDI dentro del grupo
                    var ediSegment = GenerateSegment(segmentRule, segmentJson);
                    groupSegments.Add(ediSegment);
                }
            }

            // Unir los segmentos del grupo con el separador de segmentos
            return string.Join(Definition.SegmentSeparator.ToString(), groupSegments);
        }

        private string GenerateCompositeElement(CompositeElement compositeRule, JToken segmentJson)
        {
            var subElements = new List<string>();

            // Recorrer los subelementos definidos en las reglas del elemento compuesto
            foreach (var subElementRule in compositeRule.DataElements)
            {
                if (subElementRule is DataElement dataElement)
                {
                    // Buscar el valor del subelemento en el JSON
                    var subElementValue = segmentJson[compositeRule.Name]?[dataElement.Name]?.ToString();
                    subElements.Add(subElementValue ?? ""); // Si no existe, agregar vacío
                }
            }

            // Unir los subelementos con el separador de subelementos
            return string.Join(Definition.DataElementSeparator.ToString(), subElements);
        }
    }
}