using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Contentful.SDK.ContentModel;
using Contentful.SDK.Search;
using Newtonsoft.Json;

namespace Contentful.SDK
{
    public class ContentfulClient : IContentfulClient
    {
        private HttpClient _httpClient;
        private const string ContentfulBaseUrl = "https://cdn.contentful.com";

        public string Space { get; private set; }

        public string SpaceUrl { get { return String.Format("{0}/spaces/{1}", ContentfulBaseUrl, Space); } }

        public async Task<IContentfulClient> CreateAsync(string space, string accessToken)
        {
            _httpClient = new HttpClient();
            var url = String.Format("{0}/spaces/{1}?access_token={2}", ContentfulBaseUrl, space, accessToken);
            var httpResponseMessage = await _httpClient.GetAsync(url);
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                this.Space = space;
                this.AccessToken = accessToken;
                return this;
            }
            throw new Exception();
        }


        public string AccessToken { get; private set; }

        async public Task<TEntry> GetEntryAsync<TEntry>(string id) where TEntry : IContent
        {
            if(_httpClient == null)
                throw new InvalidOperationException();
            var url = String.Format("{0}/spaces/{1}/entries/{3}?access_token={2}", ContentfulBaseUrl, Space, AccessToken, id);
            var httpResponseMessage = await _httpClient.GetAsync(url);
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                var json = await httpResponseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
                var entry = JsonConvert.DeserializeObject<TEntry>(json);
                return entry;
            }
            throw new Exception();
        }

        public async Task<ContentArray<TEntry>> GetEntriesAsync<TEntry>(IEnumerable<SearchFilter> filters) where TEntry : IContent
        {
            if (_httpClient == null)
                throw new InvalidOperationException();
            var url = String.Format("{0}/spaces/{1}/entries/?{3}&access_token={2}", ContentfulBaseUrl, Space, AccessToken, SearchFilter.GetQueryString(filters));
            var httpResponseMessage = await _httpClient.GetAsync(url);
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                var json = await httpResponseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
                var entries = JsonConvert.DeserializeObject<ContentArray<TEntry>>(json);
                return entries;
            }
            throw new Exception();
        }
    }
}
