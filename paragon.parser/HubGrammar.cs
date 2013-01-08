using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sprache;

namespace Paragon.Parser
{
    public sealed class HubGrammar : IParse<Hub>
    {
        private HubGrammar()
        {
            Identifier =
                from trailing in Parse.WhiteSpace.Many()
                from identifier in Parse.LetterOrDigit.XOr(Parse.Char('-')).XOr(Parse.Char('_')).AtLeastOnce().Text()
                from leading in Parse.WhiteSpace.Many()
                select identifier;

            Range =
                from trailing in Parse.WhiteSpace.Many()
                from min in Parse.Digit.Many().Select(x => x.Any() ? int.Parse(new string(x.ToArray())) : 0)
                from dash in Parse.Char('-').Many()
                from max in Parse.Digit.Many().Select(x => x.Any() ? int.Parse(new string(x.ToArray())) : 8)
                from leading in Parse.WhiteSpace.Many()
                select dash.Any() ? (min <= max ? Enumerable.Range(min, max - min + 1) : Enumerable.Range(max, min - max + 1)) : Enumerable.Range(min, 1);

            //Min =
            //    from trailing in Parse.WhiteSpace.Many()
            //    from min in Parse.Digit.Once().Text()
            //    from dash in Parse.Char('+')
            //    from leading in Parse.WhiteSpace.Many()
            //    select Enumerable.Range(int.Parse(min), 8 - int.Parse(min));
            
            //Max =
            //    from trailing in Parse.WhiteSpace.Many()
            //    from min in Parse.Digit.Once().Text()
            //    from dash in Parse.Char('-')
            //    from leading in Parse.WhiteSpace.Many()
            //    select Enumerable.Range(0, int.Parse(min));
            
            //Value =
            //    from trailing in Parse.WhiteSpace.Many()
            //    from min in Parse.Digit.Once().Text()
            //    from leading in Parse.WhiteSpace.Many()
            //    select Enumerable.Range(int.Parse(min), 0);

            QuotedText =
                from lead in Parse.WhiteSpace.Many()
                from open in Parse.Char('"')
                from content in Parse.CharExcept('"').Many().Text()
                from close in Parse.Char('"')
                from trail in Parse.WhiteSpace.Many()
                select content;

            Name =
                from keyword in Keyword("Name")
                from id in Identifier
                from name in QuotedText
                from trail in Parse.WhiteSpace.Many()
                select new KeyValuePair<string, string>(id, name);

            HubType =
                from keyword in Keyword("Hub-Type")
                from type in Keyword("Urban").XOr(Keyword("Wilderness")).Text()
                from trail in Parse.WhiteSpace.Many()
                select type;

            Skill =
                from skill in Keyword("Investigation").XOr(Keyword("Negociation")).XOr(Keyword("Knowledge")).Text()
                select skill;

            Line =
                from keyword in Keyword("Line")
                from text in QuotedText
                from trail in Parse.WhiteSpace.Many()
                select text;

            Lore =
                from keyword in Keyword("Lore")
                from text in QuotedText
                from skill in Having
                from value in Range
                from trail in Parse.WhiteSpace.Many()
                select new Lore { Text = text, Skill = skill, Value = value };

            DoTypeVisit =
                from identifier in KeyPair("Visit")
                select "visit/" + identifier;

            DoTypeEvent =
                from identifier in KeyPair("Event")
                select "event/" + identifier;

            DoTypeShopping =
                from identifier in KeyPair("Shop")
                select "shop/" + identifier;

            WithTypeIcon =
                from identifier in KeyPair("Icon")
                select "icon/" + identifier;

            TypeSkill =
                from keyword in Keyword("Skill")
                from identifier in Skill
                select "skill/" + identifier;

            Realm =
                from identifier in KeyPair("Realm")
                select identifier;

            Do =
                from keyword in Keyword("Do")
                from identifier in DoTypeVisit.XOr(DoTypeEvent).XOr(DoTypeShopping)
                select identifier;

            With =
                from keyword in Keyword("With")
                from identifier in WithTypeIcon
                select identifier;

            Using =
                from keyword in Keyword("Using")
                from identifier in TypeSkill
                select identifier;

            Having =
                from keyword in Keyword("Having")
                from identifier in TypeSkill
                select identifier;

            Option =
                from keyword in Keyword("Option")
                from text in QuotedText
                from withs in With.Many()
                from usings in Using.Many()
                from action in Do
                select new Option { Text = text, Action = action, With = withs.ToList(), Using = usings.ToList() };

            Hub =
                from name in Name
                from realm in Realm
                from type in HubType
                from lines in Line.AtLeastOnce()
                from lores in Lore.Many()
                from options in Option.Many()
                from locations in KeyPair("Location").Many()
                select new Hub
                {
                    Id = name.Key,
                    Name = name.Value,
                    Realm = realm,
                    HubType = type,
                    Lines = lines.ToList(),
                    Lores = lores.ToList(),
                    Options = options.ToList(),
                    Locations = locations.ToList()
                };
        }
        
        private static readonly Lazy<HubGrammar> lazy = new Lazy<HubGrammar>(() => new HubGrammar(), System.Threading.LazyThreadSafetyMode.PublicationOnly);

        public static HubGrammar Instance { get { return lazy.Value; } }

        public Parser<string> Keyword(string value)
        {
            return  from trailing in Parse.WhiteSpace.Many()
                    from keyword in Parse.String(value).Or(Parse.String(value.ToLowerInvariant())).Or(Parse.String(value.ToUpperInvariant())).Or(Parse.String(value.ToTitleCase())).Text()
                    from leading in Parse.WhiteSpace.Many()
                    select keyword;
        }

        public Parser<string> KeyPair(string value)
        {
            return from keyword in Keyword(value)
                   from identifier in Identifier
                   select identifier;
        }
        
        public readonly Parser<string> DoTypeVisit;
        public readonly Parser<string> DoTypeShopping;
        public readonly Parser<string> DoTypeEvent;
        public readonly Parser<string> Do;


        public readonly Parser<string> TypeSkill;
        public readonly Parser<string> Using;
        public readonly Parser<string> Having;

        public readonly Parser<string> Realm;

        public readonly Parser<string> Skill;

        public readonly Parser<string> WithTypeIcon;
        public readonly Parser<string> With;

        public readonly Parser<Option> Option;

        public readonly Parser<string> Identifier;
        public readonly Parser<IEnumerable<int>> Range;
        //public readonly Parser<IEnumerable<int>> Value;
        //public readonly Parser<IEnumerable<int>> Max;
        //public readonly Parser<IEnumerable<int>> Min;

        public readonly Parser<string> QuotedText;

        public readonly Parser<KeyValuePair<string, string>> Name;

        public readonly Parser<string> HubType;

        public readonly Parser<string> Line;

        public readonly Parser<Lore> Lore;

        public readonly Parser<Hub> Hub;

        public Hub ParseString(string text)
        {
            return Hub.Parse(text);
        }
    }

}
