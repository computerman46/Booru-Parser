using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booru_Parser
{
    public class Picture
    {
        public string url { get; }
        public string artist { get; }
        public string source { get; }
        public List<string> tags { get; }

        public Picture(string url, string artist, string source, List<string> tags)
        {
            this.url = url;
            this.artist = artist;
            this.source = source;
            this.tags = tags;
        }
    }
}
