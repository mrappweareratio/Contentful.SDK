using System.Collections.Generic;

namespace Contentful.SDK.ContentModel
{
    public class AssetFields
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public AssetFile File { get; set; }
    }

    public class AssetFields<TAssetDetail> : Asset where TAssetDetail : struct
    {
        public TAssetDetail Details { get; set; }
    }
}