using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Contentful.SDK.ContentModel;
using Contentful.SDK.Search;

namespace Contentful.SDK
{
    public class ContentfulHelpers
    {
        public static IEnumerable<TEntry> ConvertToEntryType<TEntry>(TEntry instance,
            IEnumerable<Entry> entriesOfType) where TEntry : Entry
        {
            return entriesOfType.OfType<TEntry>();
        }

        public static IEnumerable<TEntry> ResolveLinkedEntries<TEntry>(IEnumerable<TEntry> entries,
            IContentArray contentArray)
            where TEntry : Entry, new()
        {
            return entries.Select(x => x.Sys.Id)
                .Join(contentArray.Includes.Entry, s => s, entry => entry.Sys.Id, (s, entry) => new TEntry().From<TEntry>(entry))
                .ToList();
        }

        public static IEnumerable<dynamic> ResolveLinkedEntries(IEnumerable<dynamic> entires,
           IContentArray contentArray)
        {
            return entires
                .Join(contentArray.Includes.Entry, x => x.Sys.Id, entry => entry.Sys.Id, (original, included) => original.From(included))
                .ToList();
        }

        public static IEnumerable<Asset> ResolveLinkedAssets(IEnumerable<Asset> entires,
         IContentArray contentArray)
        {
            return entires
                .Join(contentArray.Includes.Asset, x => x.Sys.Id, entry => entry.Sys.Id, (original, included) => original.From(included))
                .ToList();
        }

        public static void ResolveLinks<TEntry>(IContentArray contentArray, IEnumerable<TEntry> entries) where TEntry : Entry
        {
            var fields = typeof(TEntry).GetRuntimeProperties()
                .FirstOrDefault(x => x.Name == "Fields" && x.DeclaringType == typeof(TEntry));

            if (fields == null)
                return;

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
                            if (linkedEntryAsTargetType == null) continue;
                            linkedEntryAsTargetType.From(linkedEntry);
                            //recursively resolve links of this entry
                            ResolveLinks(contentArray, ContentfulHelpers.ConvertToEntryType(linkedEntryAsTargetType, new List<Entry> { linkedEntryAsTargetType }));
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
