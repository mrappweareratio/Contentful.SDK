using System.Collections.Generic;
using Contentful.SDK.ContentModel;

namespace Contentful.SDK.Search
{
    public class ContentArray<TContent> where TContent : IContent
    {
        public Sys Sys { get; set; }
        public int Skip { get; set; }
        public int Limit { get; set; }
        public IEnumerable<TContent> Items { get; set; }
        public Includes Includes { get; set; }
    }

    public class Includes
    {
        public IEnumerable<Asset> Asset { get; set; }
        public IEnumerable<Entry> Entry { get; set; } 
    }
}