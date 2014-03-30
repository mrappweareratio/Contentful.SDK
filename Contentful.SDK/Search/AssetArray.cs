using System.Collections.Generic;
using Contentful.SDK.ContentModel;

namespace Contentful.SDK.Search
{
    public class AssetArray<TAsset> : IContentArray<TAsset> where TAsset : Asset, IContent
    {
        public Sys Sys { get; set; }
        public int Skip { get; set; }
        public int Limit { get; set; }
        public IEnumerable<TAsset> Items { get; set; }
        public Includes Includes { get; set; }

        public void ResolveLinks()
        {

        }
    }
}