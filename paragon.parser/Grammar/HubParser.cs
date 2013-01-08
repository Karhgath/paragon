using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irony.Parsing;
using Irony.Ast;

namespace Paragon.Parser.Grammar
{
    public class HubParser : IParse<Hub>
    {
        public Hub ParseString(string text)
        {
            var grammar = new HubGrammar();
            var language = new LanguageData(grammar);

            var parser = new Irony.Parsing.Parser(language);

            var tree = parser.Parse(text);

            var hub = new Hub
            {
                Id = tree.Root.ChildNodes.Single(x => x.Term.Name == "Name").ChildNodes[0].Token.ValueString,
                Name = tree.Root.ChildNodes.Single(x => x.Term.Name == "Name").ChildNodes[1].Token.ValueString,
                HubType = tree.Root.ChildNodes.Single(x => x.Term.Name == "HubType").ChildNodes[0].Token.ValueString,
                Lines = new List<string>(tree.Root.ChildNodes.Where(x => x.Term.Name == "Line").Select(x => x.ChildNodes[0].Token.ValueString)),
                Lores = new List<string>(tree.Root.ChildNodes.Where(x => x.Term.Name == "Lore").Select(x => x.ChildNodes[0].Token.ValueString)),
            };

            return hub;
        }
    }
}
