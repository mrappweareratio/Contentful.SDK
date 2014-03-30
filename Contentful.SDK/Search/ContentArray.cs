using System.Collections;
using System.Collections.Generic;
using Contentful.SDK.ContentModel;

namespace Contentful.SDK.Search
{
    public interface IContentArray<TContent> : IContentArray where TContent : IContent
    {
        IEnumerable<TContent> Items { get; set; }
        void ResolveLinks();
    }

    public interface IContentArray
    {
        Sys Sys { get; set; }
        int Skip { get; set; }
        int Limit { get; set; }
        Includes Includes { get; set; }
    }

    public class Includes
    {
        public IEnumerable<Asset> Asset { get; set; }
        public IEnumerable<Entry> Entry { get; set; }
    }
}