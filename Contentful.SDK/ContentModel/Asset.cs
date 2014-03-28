namespace Contentful.SDK.ContentModel
{
    public class Asset
    {
        public Sys Sys { get; set; }
        public AssetFields Fields { get; set; }
    }

    public class Asset<TAssetDetail> : Asset where TAssetDetail : struct
    {
        public new AssetFields<TAssetDetail> Fields { get; set; }
    }
}