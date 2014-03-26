using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Contentful.SDK.Search
{
    public class SearchFilter : Dictionary<string, string>
    {
        public static string GetQueryString(IEnumerable<SearchFilter> filters)
        {
            return String.Join("&", filters.Select(x => x.GetQueryString()));
        }

        protected string GetQueryString()
        {
            var builder = new List<string>(Count);
            builder.AddRange(this.Select(p => String.Format("{0}={1}", p.Key, p.Value)));
            return String.Join("&", builder.ToString());
        }
        protected string Get(string key)
        {
            return this.ContainsKey(key) ? this[key] : null;
        }

        protected void Set(string key, string value)
        {
            this[key] = value;
        }
    }

    public class ContentTypeSearchFilter : SearchFilter
    {
        public string ContentType { get { return Get("content_type"); } set { Set("content_type", value); } }
    }
}