using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using Contentful.SDK;
using Contentful.SDK.ContentModel;
using Contentful.SDK.Search;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;

namespace UnitTests
{
    [TestClass]
    public class ContentfulClientUnitTest
    {
        private const string AccessToken = "b4c0n73n7fu1";
        private const string Space = "cfexampleapi";

        private async Task<IContentfulClient> CreateClientAsync()
        {
            IContentfulClient client = new ContentfulClient();
            client = await client.CreateAsync(Space, AccessToken);
            return client;
        }

        [TestMethod]
        public async Task Create()
        {
            var client = await CreateClientAsync();
            Assert.IsNotNull(client);
            Assert.IsNotNull(client.Space);
        }

        [TestMethod]
        public async Task GetEntryAsync()
        {
            var client = await CreateClientAsync();
            var entry = await client.GetEntryAsync<Entry>("nyancat");
            Assert.IsNotNull(entry);
            Assert.AreEqual(entry.Sys.Id, "nyancat");
            Assert.AreNotEqual(DateTime.MinValue, entry.Sys.CreatedAt);
            Assert.AreNotEqual(DateTime.MinValue, entry.Sys.UpdatedAt);
            Assert.AreEqual(SysType.Entry, entry.Sys.Type);
        }

        [TestMethod]
        public async Task GetEntriesAsync()
        {
            var client = await CreateClientAsync();
            var entries = await client.GetEntriesAsync<Cat>(new List<SearchFilter>
            {
                new ContentTypeSearchFilter("cat")
            });
            Assert.IsNotNull(entries);
            Assert.AreEqual(SysType.Array, entries.Sys.Type);
            Assert.IsTrue(entries.Items.Any());
            Assert.IsTrue(entries.Items.ToList().TrueForAll(x => !String.IsNullOrEmpty(x.Name)), "Names not empty");

        }

        [TestMethod]
        public async Task GetEntriesSortedAsync()
        {
            var client = await CreateClientAsync();
            var entries = await client.GetEntriesAsync<Cat>(new List<SearchFilter>
            {
                new ContentTypeSearchFilter("cat"),
                new OrderSearchOption()
                {
                    Order = "sys.createdAt"
                }
            });
            Assert.IsNotNull(entries);
            var list = entries.Items.ToArray();
            for (int i = 0; i < list.Length; i++)
            {
                if (i == 0) continue;
                Assert.IsTrue(list[i].Sys.CreatedAt >= list[i - 1].Sys.CreatedAt, "CreatedAt Ascending");
            }
        }

        [TestMethod]
        public async Task GetEntriesSortedDescAsync()
        {
            var client = await CreateClientAsync();
            var entries = await client.GetEntriesAsync<Cat>(new List<SearchFilter>
            {
                new ContentTypeSearchFilter("cat"),
                new OrderSearchOption()
                {
                    Order = "sys.createdAt", Descending = true
                }
            });
            Assert.IsNotNull(entries);
            var list = entries.Items.ToArray();
            for (int i = 0; i < list.Length; i++)
            {
                if (i == 0) continue;
                Assert.IsTrue(list[i].Sys.CreatedAt <= list[i - 1].Sys.CreatedAt, "CreatedAt Descending");
            }


        }

        [TestMethod]
        public async Task GetEntriesLimitAsync()
        {
            var client = await CreateClientAsync();
            var entries = await client.GetEntriesAsync<Cat>(new List<SearchFilter>
            {
                new ContentTypeSearchFilter("cat"),
                new OrderSearchOption()
                {
                    Order = "sys.createdAt", Descending = true
                },
                new PaginationSearchOption(null, 3)
            });
            Assert.IsNotNull(entries);
            Assert.AreEqual(3, entries.Limit);
            var list = entries.Items.ToArray();
            Assert.AreEqual(entries.Limit, list.Length);
        }

        [TestMethod]
        public async Task GetEntriesEqualityAsync()
        {
            var client = await CreateClientAsync();
            var entries = await client.GetEntriesAsync<Cat>(new List<SearchFilter>
            {
                new EqualitySearchFilter("sys.id", "nyancat")
            });
            Assert.IsNotNull(entries);
            var list = entries.Items.ToArray();
            Assert.AreEqual(list[0].Sys.Id, "nyancat");
        }

        [TestMethod]
        public async Task GetEntriesEqualityIncludesAssetAsync()
        {
            var client = await CreateClientAsync();
            var entries = await client.GetEntriesAsync<Cat>(new List<SearchFilter>
            {
                new EqualitySearchFilter("sys.id", "nyancat"),
                new IncludeLinksSearchOption(2)
            });
            Assert.IsNotNull(entries);
            var list = entries.Items.ToArray();
            Assert.AreEqual(list[0].Sys.Id, "nyancat");
            Assert.IsNotNull(entries.Includes);
            Assert.IsNotNull(entries.Includes.Asset);
            Assert.IsTrue(entries.Includes.Asset.ToList().TrueForAll(x => !String.IsNullOrEmpty(x.Fields.File.Url)));
        }


        public class Cat : Entry
        {
            public string Name
            {
                get { return GetField<string>("name"); }
            }
        }
    }
}
