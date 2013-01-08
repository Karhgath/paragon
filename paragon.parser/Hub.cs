using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paragon.Parser
{
    public class Hub
    {
        public string Realm { get; set; }

        public string Name { get; set; }

        public string HubType { get; set; }

        public List<string> Lines { get; set; }

        public List<Lore> Lores { get; set; }

        public string Id { get; set; }

        public List<Option> Options { get; set; }

        public List<string> Locations { get; set; }
    }

    public class Option
    {
        public string Text { get; set; }
        public string Action { get; set; }
        public List<string> With { get; set; }
        public List<string> Using { get; set; }
        public string Icon { get { return With.Where(x => x.StartsWith("icon/")).Select(x => x.Split('/')[1]).Single() ?? "icon-home"; } }
    }

    public class Lore
    {
        public string Text { get; set; }
        public string Skill { get; set; }
        public IEnumerable<int> Value { get; set; }
    }
}
