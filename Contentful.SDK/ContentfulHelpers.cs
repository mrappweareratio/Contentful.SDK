using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contentful.SDK.ContentModel;

namespace Contentful.SDK
{
    public class ContentfulHelpers
    {
        public static IEnumerable<TEntry> ConvertToEntryType<TEntry>(TEntry empty,
            IEnumerable<Entry> entriesOfType) where TEntry : class
        {
            var list = new List<TEntry>();
            foreach (var obj in entriesOfType)
            {
                var entry = obj as TEntry;
                if (entry == null) continue;
                list.Add(entry);
            }
            return list;
        }

        public static IEnumerable<TEntry> BuildFromIncludesEntries<TEntry>(IEnumerable<TEntry> entires,
            IEnumerable<Entry> includedEntries)
            where TEntry : Entry, new()
        {
            return entires.Select(x => x.Sys.Id)
                .Join(includedEntries, s => s, entry => entry.Sys.Id, (s, entry) => new TEntry().From<TEntry>(entry))
                .ToList();
        }

        public static IEnumerable<dynamic> BuildFromIncludesEntries<TEntry>(IEnumerable<dynamic> entires,
           IEnumerable<Entry> includedEntries)
        {
            return entires
                .Join(includedEntries, x => x.Sys.Id, entry => entry.Sys.Id, (original, included) => original.From(included))
                .ToList();
        }
    }
}
