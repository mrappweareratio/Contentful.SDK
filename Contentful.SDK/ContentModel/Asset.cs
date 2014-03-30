using System;
using Contentful.SDK.Search;

namespace Contentful.SDK.ContentModel
{
    public class Asset : IContent
    {
        public Sys Sys { get; set; }
        public AssetFields Fields { get; set; }
        public string Url { get { return Fields.File.Url; } }

        /// <summary>
        /// Images are hosted on images.contentful.com. For files on this host you can attach the URI query parameters w and/or h to specify the desired dimensions. The image will never be stretched, skewed or enlarged. Instead it will be fit into the bounding box given by the w and h parameters.
        ///Additionaly, a q can be passed to define the JPEG compression quality between 1 and 100 and the fm parameter can be used to change the format to either "png" or "jpg".
        /// </summary>
        public string GetImageUrl(ImageOptions options)
        {
            return String.Format("{0}?{1}", Url, options.GetQueryString());
        }

        /// <summary>
        /// Images are hosted on images.contentful.com. For files on this host you can attach the URI query parameters w and/or h to specify the desired dimensions. The image will never be stretched, skewed or enlarged. Instead it will be fit into the bounding box given by the w and h parameters.
        ///Additionaly, a q can be passed to define the JPEG compression quality between 1 and 100 and the fm parameter can be used to change the format to either "png" or "jpg".
        /// </summary>
        public class ImageOptions : SearchFilter
        {
            public int Width
            {
                get { return Convert.ToInt32(Get("w")); }
                set { Set("w", value.ToString()); }
            }

            public int Height
            {
                get { return Convert.ToInt32(Get("h")); }
                set { Set("h", value.ToString()); }
            }

            public int Quality
            {
                get { return Convert.ToInt32(Get("q")); }
                set
                {
                    if (value < 1 || value > 100) throw new ArgumentOutOfRangeException();
                    Set("q", value.ToString());
                }
            }

            public ImageFormat Format
            {
                get
                {
                    ImageFormat format;
                    return Enum.TryParse(Get("fm"), true, out format) ? format : ImageFormat.Jpg;
                }
                set { Set("fm", value.ToString().ToLower()); }
            }

            public enum ImageFormat
            {
                Jpg,
                Png
            }
        }

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