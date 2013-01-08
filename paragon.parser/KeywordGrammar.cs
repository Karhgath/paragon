using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sprache;

namespace Paragon.Parser
{
    public class Fragment
    {
        public Fragment(string text)
        {
            Text = text;
            Keywords = new List<string>();
        }

        public Fragment(string text, string keyword)
        {
            Text = text + keyword;
            Keywords = new List<string> { keyword };
        }

        public Fragment Append(Fragment next)
        {
            Text = Text + next.Text;
            Keywords.AddRange(next.Keywords);

            return this;
        }

        public string Text { get; set; }
        public List<string> Keywords { get; set; }
    }

    public sealed class KeywordGrammar : IParse<Fragment>
    {
        private static readonly Lazy<KeywordGrammar> lazy = new Lazy<KeywordGrammar>(() => new KeywordGrammar(), System.Threading.LazyThreadSafetyMode.PublicationOnly);

        public static KeywordGrammar Instance { get { return lazy.Value; } }

        private KeywordGrammar()
        {
            Text =
                from text in Parse.AnyChar.Many().Text()
                select new Fragment(text);

            Keyword =
                from leading in Parse.AnyChar.Until(Parse.String("$(").Once()).Text()
                from keyword in Parse.AnyChar.Until(Parse.String(")").Once()).Text()
                select new Fragment(leading, keyword);

            All =
                from fragments in Keyword.Many()
                from end in Text
                select fragments.Aggregate((x, y) => x.Append(y)).Append(end);
        }


        public readonly Parser<Fragment> Text;
        public readonly Parser<Fragment> Keyword;
        public readonly Parser<Fragment> All;


        public Fragment ParseString(string text)
        {
            return All.Parse(text);
        }
    }
}
