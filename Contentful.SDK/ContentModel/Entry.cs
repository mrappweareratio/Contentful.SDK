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

        public virtual TEntry From<TEntry>(Entry entry) where TEntry : Entry
        {
            if(!(this is TEntry))
                throw new InvalidOperationException(String.Format("Cannot convert Entryy of type {0} to type {1}", this.GetType(), typeof(TEntry)));
            Fields = entry.Fields;
            Sys = entry.Sys;
            return this as  TEntry;
        }
    }
}