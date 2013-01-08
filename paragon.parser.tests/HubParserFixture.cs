using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sprache;
using Xunit;
namespace Paragon.Parser.Tests
{
    public class HubParserFixture
    {
        [Fact]
        public void ParseIdentifierNoSpace()
        {
            var result = HubGrammar.Instance.Identifier.Parse("test");

            Assert.Equal<string>("test", result);
        }

        [Fact]
        public void ParseIdentifierWithWhitespace()
        {
            var result = HubGrammar.Instance.Identifier.Parse("  test   ");

            Assert.Equal<string>("test", result);
        }

        [Fact]
        public void ParseIdentifierWithOneDash()
        {
            var result = HubGrammar.Instance.Identifier.Parse("test-one");

            Assert.Equal<string>("test-one", result);
        }
        
        [Fact]
        public void ParseIdentifierWithMultipleDashAndWhitespace()
        {
            var result = HubGrammar.Instance.Identifier.Parse("  test-one-two-three   ");

            Assert.Equal<string>("test-one-two-three", result);
        }

        [Fact]
        public void ParseIdentifierWithSlash()
        {
            var result = HubGrammar.Instance.Identifier.Parse("test/slash");

            Assert.Equal<string>("test", result);
        }

        [Fact]
        public void ParseIdName()
        {
            var result = HubGrammar.Instance.Name.Parse("NAME test-id \"This is a name\"");

            Assert.Equal<string>("test-id", result.Key);
            Assert.Equal<string>("This is a name", result.Value);
        }

        [Fact]
        public void ParseIdNameNoSpace()
        {
            var result = HubGrammar.Instance.Name.Parse("name test-id \"This is a name\"");

            Assert.Equal<string>("test-id", result.Key);
            Assert.Equal<string>("This is a name", result.Value);
        }

        [Fact]
        public void ParseHubTypeUrban()
        {
            var result = HubGrammar.Instance.HubType.Parse("hub-type urban");

            Assert.Equal<string>("urban", result);
        }

        [Fact]
        public void ParseHubTypeWilderness()
        {
            var result = HubGrammar.Instance.HubType.Parse("hub-type wilderness");

            Assert.Equal<string>("wilderness", result);
        }

        [Fact]
        public void ParseHubTypeInvalidThrows()
        {
            Assert.Throws<Sprache.ParseException>(() => HubGrammar.Instance.HubType.Parse("hub-type none"));
        }

        [Fact]
        public void ParseIdNameInvertedThrows()
        {
            Assert.Throws<Sprache.ParseException>(() => HubGrammar.Instance.Name.Parse("name \"This is a name\" test-id"));
        }

        [Fact]
        public void ParseOneLine()
        {
            var result = HubGrammar.Instance.Line.Parse("line \"abcde\"");

            Assert.Equal<string>("abcde", result);
        }

        [Fact]
        public void ParseTwoLine()
        {
            var result = HubGrammar.Instance.Line.AtLeastOnce().Parse("line \"abcde\"\nline \"wxyz\"").ToList();

            Assert.Equal<int>(2, result.Count());
            Assert.Equal<string>("abcde", result[0]);
            Assert.Equal<string>("wxyz", result[1]);
        }


        [Fact]
        public void ParseNoLineThrows()
        {
            Assert.Throws<Sprache.ParseException>(() => HubGrammar.Instance.Line.AtLeastOnce().Parse("").ToList());
        }


        [Fact]
        public void CreateComplexParser()
        {
            var parser =
                from name in HubGrammar.Instance.Name
                from type in HubGrammar.Instance.HubType
                from lines in HubGrammar.Instance.Line.AtLeastOnce()
                from mores in HubGrammar.Instance.Lore.Many()
                select new Hub
                {
                    Id = name.Key,
                    Name = name.Value,
                    HubType = type,
                    Lines = lines.ToList(),
                    Lores = mores.ToList()
                };

            var result = parser.Parse("name test-id \"This is a name\"\nhub-type urban\nline \"test line\"\nline \"second line\"\nmore \"and more\"");

            Assert.NotNull(result);
        }

        [Fact]
        public void ParseHub()
        {
            var hub = HubGrammar.Instance.Hub.Parse("name test-id \"This is a name\"\nrealm test\nhub-type urban\nline \"test line\"\nline \"second line\"\nlore \"and more\" having skill knowledge 4-6");

            Assert.Equal<string>("test-id", hub.Id);
            Assert.Equal<string>("This is a name", hub.Name);
            Assert.Equal<string>("test", hub.Realm);
            Assert.Equal<string>("urban", hub.HubType);
            Assert.Equal<int>(2, hub.Lines.Count);
            Assert.Equal<string>("test line", hub.Lines[0]);
            Assert.Equal<string>("second line", hub.Lines[1]);
            Assert.Equal<int>(1, hub.Lores.Count);
            Assert.Equal<string>("and more", hub.Lores[0].Text);
        }
    }
}
