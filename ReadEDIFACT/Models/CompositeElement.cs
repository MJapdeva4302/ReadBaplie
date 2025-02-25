namespace ReadEDIFACT;

public class CompositeElement : Element
    {
        public IEnumerable<Element> DataElements { get; set; }

        public override string ToString()
        {
            return string.Format("Composite element: {0}", Name);
        }
    }
