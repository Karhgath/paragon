using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paragon.Parser
{
    public interface IParse<T>
    {
        T ParseString(string text);
    }
}
