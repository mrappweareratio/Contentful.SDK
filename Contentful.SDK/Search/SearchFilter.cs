using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Contentful.SDK.Search
{
    public class SearchFilter
    {
        private readonly Dictionary<string, string> _pairs = new Dictionary<string, string>();

        public static string GetQueryString(IEnumerable<SearchFilter> filters)
        {
            return String.Join("&", filters.Select(x => x.GetQueryString()));
        }

        protected virtual string GetQueryString()
        {
            return String.Join("&", _pairs.Where(p => p.Value != null).Select(p => String.Format("{0}={1}", p.Key, p.Value)));
        }

        protected string Get(string key)
        {
            return _pairs.ContainsKey(key) ? _pairs[key] : null;
        }

        protected void Set(string key, string value)
        {
            _pairs[key] = value;
        }
    }
}