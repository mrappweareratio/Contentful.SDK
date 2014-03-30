namespace Contentful.SDK.ContentModel
{
    public class Asset : IContent
    {
        public Sys Sys { get; set; }
        public AssetFields Fields { get; set; }
        public string Url { get { return Fields.File.Url; } }

        public Asset From(Asset included)
        {
            Sys = included.Sys;
            Fields = included.Fields;
            return this;
        }
    }

    public class Asset<TAssetDetail> : Asset where TAssetDetail : struct
    {
        public new AssetFields<TAssetDetail> Fields { get; set; }
    }
}