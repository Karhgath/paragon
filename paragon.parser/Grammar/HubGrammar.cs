using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irony.Parsing;
using Irony.Ast;

namespace Paragon.Parser.Grammar
{
    public class HubGrammar : Irony.Parsing.Grammar
    {
        public HubGrammar() : base(false)
        {
            var identifier = new Irony.Parsing.IdentifierTerminal("identifier", "-");
            var text = new Irony.Parsing.QuotedValueLiteral("text", "\"", TypeCode.String);

            var DoTypeVisit = new NonTerminal("DoTypeVisit");
            var DoTypeShopping = new NonTerminal("DoTypeShopping");
            var DoTypeEvent = new NonTerminal("DoTypeEvent");
            var DoType = new NonTerminal("DoType");
            var Do = new NonTerminal("Do");

            var Skill = new NonTerminal("Skill");
            var UsingTypeSkill = new NonTerminal("UsingTypeSKill");
            var UsingType = new NonTerminal("UsingType");
            var Using = new NonTerminal("Using");

            var WithTypeIcon = new NonTerminal("WithTypeIcon");
            var WithType = new NonTerminal("WithType");
            var With = new NonTerminal("With");

            var Option = new NonTerminal("Option");
            var OptionList = new NonTerminal("OptionList");
            
            var More = new NonTerminal("More");
            var Line = new NonTerminal("Line");
            var DescriptionList = new NonTerminal("DescriptionList");

            var HubTypeValues = new NonTerminal("HubTypeValues");
            var HubType = new NonTerminal("HubType");
            var Name = new NonTerminal("Name");
            
            var Hub = new NonTerminal("Hub");

            Root = Hub;
            

            Hub.Rule = Name + HubType + DescriptionList + OptionList;
            Name.Rule = ToTerm("name") + identifier + text;
            HubType.Rule = ToTerm("hub-type") + HubTypeValues;
            HubTypeValues.Rule = ToTerm("urban") | "wilderness";
            
            DescriptionList.Rule = MakePlusRule(DescriptionList, null, Line) + MakeStarRule(DescriptionList, null, More);
            Line.Rule = ToTerm("line") + text;
            More.Rule = ToTerm("more") + text;
            
            OptionList.Rule = MakePlusRule(OptionList, null, Option);
            Option.Rule = ToTerm("option") + text + MakeStarRule(Option, null, With) + MakeStarRule(Option, null, Using) + Do;

            With.Rule = ToTerm("with") + WithType;
            WithType.Rule = WithTypeIcon;
            WithTypeIcon.Rule = ToTerm("icon") + identifier;

            Using.Rule = ToTerm("using") + UsingType;
            UsingType.Rule = UsingTypeSkill;
            UsingTypeSkill.Rule = ToTerm("skill") + Skill;
            Skill.Rule = ToTerm("investigation") | "negociation";

            Do.Rule = ToTerm("do") + DoType;
            DoType.Rule = DoTypeEvent | DoTypeShopping | DoTypeVisit;
            DoTypeEvent.Rule = ToTerm("event") + identifier;
            DoTypeShopping.Rule = ToTerm("shopping") + identifier;
            DoTypeVisit.Rule = ToTerm("visit") + identifier;

            MarkPunctuation("name", "line", "more", "option", "with", "icon", "using", "skill", "do", "event", "shopping", "visit");

            LanguageFlags = Irony.Parsing.LanguageFlags.CreateAst | LanguageFlags.NewLineBeforeEOF;
        }
    }
}
