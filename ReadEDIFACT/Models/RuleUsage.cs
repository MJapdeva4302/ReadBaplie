using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReadEDIFACT
{
    public enum RuleUsage
    {
        Mandatory = 'M',
        Required = 'R',
        Conditional = 'C',
        Dependent = 'D',
        Recommended = 'A',
        Optional = 'O',
        NotUsed = 'X'
    }
}