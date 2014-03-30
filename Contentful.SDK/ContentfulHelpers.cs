using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    }
}
