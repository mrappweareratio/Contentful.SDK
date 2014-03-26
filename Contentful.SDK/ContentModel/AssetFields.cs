using System.Collections.Generic;

namespace Contentful.SDK.ContentModel
{
    public class AssetFields
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public IEnumerable<AssetFile> File { get; set; } 
    }
}