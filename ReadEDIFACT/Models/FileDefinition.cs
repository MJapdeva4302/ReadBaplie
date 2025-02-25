using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReadEDIFACT
{
    public class FileDefinition
    {
        public string Name { get; set; }
        public Version Version { get; set; }
        public char SegmentSeparator { get; set; }
        public char ElementSeparator { get; set; }
        public char DataElementSeparator { get; set; }
        public char EscapeCharacter { get; set; }

        public IEnumerable<Segment> Segments { get; set; }

        public FileDefinition(string name)
        {
            Name = name;
        }

        public FileDefinition(string name, Version version)
        {
            Name = name;
            Version = version;
        }

        public override string ToString()
        {
            string output = string.Empty;

            output += "File Definition\n\n";

            output += string.Format("{0}, Version: {1} \nSegment Separator: {2} \nElement Separator: {3} \nData Element Separator: {4} \nEscape Character: {5}",
                Name, Version, SegmentSeparator, ElementSeparator, DataElementSeparator, EscapeCharacter);
            output += "\n\nSegments:\n\n";

            output += OutputSegments(Segments);

            return output;
        }

        public string OutputSegments(IEnumerable<Segment> segments, int level = 1)
        {
            string output = string.Empty;

            foreach (var segment in segments)
            {
                if (segment.GetType() == typeof(SegmentData))
                {
                    string trail = "";
                    for (int i = 0; i < level; i++)
                    {
                        trail += "-";
                    }
                    output += string.Format("|{0} {1}\n", trail, segment);

                    output += OutputElements((((SegmentData)segment).DataElements), level + 1);
                    output += string.Format("|{0} {1}\n", trail, segment.SegmentID);

                }
                else if (segment.GetType() == typeof(SegmentGroup))
                {
                    output += OutputSegments(((SegmentGroup)segment).Segments, level + 1);
                }

            }

            return output;
        }

        public string OutputElements(IEnumerable<Element> elements, int level = 0)
        {
            string output = string.Empty;

            foreach (var element in elements)
            {
                string trail = "";
                for (int i = 0; i < level; i++)
                {
                    trail += "-";
                }

                if (element.GetType() == typeof(DataElement))
                {                    
                    output += string.Format("|{0} {1}\n", trail, element);
                }
                else if (element.GetType() == typeof(EmptyElement))
                {
                    output += string.Format("|{0} {1}\n", trail, element);                    
                }
                else if (element.GetType() == typeof(CompositeElement))
                {
                    output += string.Format("|{0} {1}\n", trail, element);
                    output += OutputElements(((CompositeElement)element).DataElements, level + 1);                    
                }

            }

            return output;
        }
    }
}