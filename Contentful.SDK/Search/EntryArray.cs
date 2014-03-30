using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Contentful.SDK.ContentModel;

namespace Contentful.SDK.Search
{
    public class EntryArray<TContent> : IContentArray<TContent> where TContent : Entry
    {
        public Sys Sys { get; set; }
        public int Skip { get; set; }
        public int Limit { get; set; }
        public IEnumerable<TContent> Items { get; set; }
        public Includes Includes { get; set; }

        public void ResolveLinks()
        {
            ContentfulHelpers.ResolveLinks(this, Items);
        }       
    }
}