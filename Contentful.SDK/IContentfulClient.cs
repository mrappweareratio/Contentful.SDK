using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Contentful.SDK.ContentModel;
using Contentful.SDK.Search;

namespace Contentful.SDK
{
    public interface IContentfulClient
    {
        string Space { get; }
        Task<IContentfulClient> CreateAsync(string space, string accessToken);
        Task<TEntry> GetEntryAsync<TEntry>(string id) where TEntry : IContent;
        Task<ContentArray<TEntry>> GetEntriesAsync<TEntry>(IEnumerable<SearchFilter> filters) where TEntry : IContent;
    }
}