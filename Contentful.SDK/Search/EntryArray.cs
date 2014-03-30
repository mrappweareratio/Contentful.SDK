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
            ResolveLinks(this, Items);
        }

        private void ResolveLinks(IContentArray<TContent> contentArray, IEnumerable<Entry> entries)
        {
            var fields = typeof (TContent).GetRuntimeProperties()
                .FirstOrDefault(x => typeof(TContent) == typeof(Entry) 
                    ? x.Name == "Fields"
                    : x.Name == "Fields" && x.DeclaringType == typeof(TContent));

            var props = fields.PropertyType.GetRuntimeProperties()
                .Where(p => p.GetCustomAttributes(typeof(LinkedContentAttribute), true).Any()).ToList();
            
            foreach (var prop in props)
            {
                var attribute =
                    prop.GetCustomAttribute(typeof(LinkedContentAttribute), true) as LinkedContentAttribute;

                foreach (var entry in entries)
                {
                    //get links
                    var linkedContent = prop.GetValue(fields.GetValue(entry)) as IContent;
                    if (linkedContent == null) continue;
                    switch (linkedContent.Sys.LinkType)
                    {
                        case LinkType.Entry:
                            var linkedEntry = contentArray.Includes.Entry.FirstOrDefault(x => x.Sys.Id == linkedContent.Sys.Id);
                            if (linkedEntry == null) continue;
                            dynamic linkedEntryAsTargetType = Activator.CreateInstance(attribute.TargetType) as Entry;
                            if(linkedEntryAsTargetType == null) continue;
                            linkedEntryAsTargetType.From(linkedEntry);
                            //recursively resolve links of this entry
                            ResolveLinks(contentArray, new List<Entry> { linkedEntryAsTargetType });
                            prop.SetValue(fields.GetValue(entry), linkedEntryAsTargetType);
                            break;
                        case LinkType.Asset:
                            var asset = contentArray.Includes.Asset.FirstOrDefault(x => x.Sys.Id == linkedContent.Sys.Id);
                            if (asset == null) continue;
                            prop.SetValue(fields.GetValue(entry), asset);
                            break;
                    }
                }
            }

            props = fields.PropertyType.GetRuntimeProperties()
                .Where(p => p.GetCustomAttributes(typeof(LinkedContentArrayAttribute), true).Any())
                .ToList();

            foreach (var prop in props)
            {
                var attribute =
                    prop.GetCustomAttribute(typeof(LinkedContentArrayAttribute), true) as LinkedContentArrayAttribute;

                foreach (var entry in entries)
                {
                    if (attribute == null) continue;
                    var propValue = prop.GetValue(fields.GetValue(entry));
                    switch (attribute.LinkType)
                    {
                        case LinkType.Entry:
                            //find list of linked entries
                            var arrayAsEntries = propValue as IEnumerable<Entry>;
                            dynamic target = Activator.CreateInstance(attribute.TargetType);
                            var arrayAsTargetType = ContentfulHelpers.ConvertToEntryType(target, arrayAsEntries);
                            //resolve recursive links within linked entries
                            ResolveLinks(contentArray, arrayAsTargetType);
                            var value = ContentfulHelpers.ResolveLinkedEntries(arrayAsTargetType, contentArray);                            
                            prop.SetValue(fields.GetValue(entry), value);
                            break;

                        case LinkType.Asset:
                            //find list of linked assets
                            var assets = propValue as IEnumerable<Asset>;
                            if (assets == null)
                                continue;
                            var linkedAssets = ContentfulHelpers.ResolveLinkedAssets(assets, contentArray);
                            prop.SetValue(fields.GetValue(entry), linkedAssets);
                            break;
                    }
                }
            }
        }
    }
}