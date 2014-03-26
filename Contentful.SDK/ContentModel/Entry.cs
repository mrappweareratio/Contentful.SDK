using System;
using Newtonsoft.Json.Linq;

namespace Contentful.SDK.ContentModel
{
    public class Entry : IEntry
    {
        public Sys Sys { get; set; }

        public JObject Fields { get; set; }

        public T GetField<T>(string propertyName)
        {
            if (Fields == null) throw new InvalidOperationException();
            JToken token;
            return Fields.TryGetValue(propertyName, out token) ? token.ToObject<T>() : default(T);
        }
    }
}