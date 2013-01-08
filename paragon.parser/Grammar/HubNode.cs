using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irony.Parsing;
using Irony.Ast;

namespace Paragon.Parser.Grammar
{
    public class HubNode : IAstNodeInit
    {
        public HubNode()
        {
        }

        public void Init(AstContext context, ParseTreeNode parseNode)
        {
            throw new NotImplementedException();
        }

        public string Name { get; set; }
    }
}
