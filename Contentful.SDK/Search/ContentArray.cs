using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Contentful.SDK.ContentModel;

namespace Contentful.SDK.Search
{
    public interface IContentArray<TContent> where TContent : IContent
    {
        Sys Sys { get; set; }
        int Skip { get; set; }
        int Limit { get; set; }
        IEnumerable<TContent> Items { get; set; }
        Includes Includes { get; set; }
        void ResolveLinks();
    }

    public class AssetArray<TAsset> : IContentArray<TAsset> where TAsset : Asset, IContent
    {
        public Sys Sys { get; set; }
        public int Skip { get; set; }
        public int Limit { get; set; }
        public IEnumerable<TAsset> Items { get; set; }
        public Includes Includes { get; set; }

        public void ResolveLinks()
        {

        }
    }

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

        private void ResolveLinks(IContentArray<TContent> contentArray, IEnumerable<Entry> items)
        {
            var fields = typeof (TContent).GetRuntimeProperties()
                .FirstOrDefault(x => typeof(TContent) == typeof(Entry) 
                    ? x.Name == "Fields"
                    : x.Name == "Fields" && x.DeclaringType == typeof(TContent));

            var props = fields.PropertyType.GetRuntimeProperties()
                .Where(p => p.GetCustomAttributes(typeof(LinkedContentAttribute), true).Any()).ToList();
            foreach (var prop in props)
            {
                foreach (var item in items)
                {

                    //get links
                    var linkedContent = prop.GetValue(fields.GetValue(item)) as IContent;
                    if (linkedContent == null) continue;
                    switch (linkedContent.Sys.LinkType)
                    {
                        case LinkType.Entry:
                            var entry = contentArray.Includes.Entry.FirstOrDefault(x => x.Sys.Id == linkedContent.Sys.Id);
                            if (entry == null) continue;
                            ResolveLinks(contentArray, new List<Entry> { entry });
                            prop.SetValue(item, entry);
                            break;
                        case LinkType.Asset:
                            var asset = contentArray.Includes.Asset.Where(x => x.Sys.Id == linkedContent.Sys.Id);
                            prop.SetValue(item, asset);
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

                foreach (var item in items)
                {
                    if (attribute == null) continue;
                    switch (attribute.LinkType)
                    {
                        case LinkType.Entry:
                            var propValue = prop.GetValue(fields.GetValue(item));
                            dynamic target = Activator.CreateInstance(attribute.TargetType);
                            var array = propValue as IEnumerable<Entry>;
                            var linkedContentArray = ContentfulHelpers.ConvertToEntryType(target, array);
                            var value = ContentfulHelpers.BuildFromIncludesEntries(linkedContentArray,
                                contentArray.Includes.Entry);
                            prop.SetValue(fields.GetValue(item), value);
                            break;

                        case LinkType.Asset:
                            break;
                    }
                }
            }
        }
    }

    public class Includes
    {
        public IEnumerable<Asset> Asset { get; set; }
        public IEnumerable<Entry> Entry { get; set; }
    }
}