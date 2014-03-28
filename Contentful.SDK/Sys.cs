using System;
using Contentful.SDK.ContentModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Contentful.SDK
{
    public class Sys
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public SysType Type { get; set; }
             [JsonConverter(typeof(StringEnumConverter))]
        public LinkType LinkType { get; set; }
        public string Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public enum LinkType
    {
        Entry,
        Asset
    }
}