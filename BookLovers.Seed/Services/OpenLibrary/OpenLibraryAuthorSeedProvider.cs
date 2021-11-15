using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;
using BookLovers.Base.Infrastructure.Extensions;
using BookLovers.Seed.Models;
using BookLovers.Seed.Models.OpenLibrary.Authors;
using BookLovers.Seed.Models.OpenLibrary.Books;
using Newtonsoft.Json;

namespace BookLovers.Seed.Services.OpenLibrary
{
    internal class OpenLibraryAuthorSeedProvider : ISeedProvider<SeedAuthor>, ISeedProvider
    {
        private static readonly string _authorUrl = "https://openlibrary.org";

        private readonly IMapper _mapper;
        private readonly BookRootsAccessor _bookRootsAccessor;
        private readonly HttpClient _httpClient;

        public SeedProviderType ProviderType => SeedProviderType.Authors;

        public SourceType SourceType => SourceType.OpenLibrary;

        public OpenLibraryAuthorSeedProvider(IMapper mapper, BookRootsAccessor bookRootsAccessor)
        {
            this._mapper = mapper;
            this._bookRootsAccessor = bookRootsAccessor;
            this._httpClient = HttpClientFactory.Create();
        }

        public async Task<IEnumerable<SeedAuthor>> ProvideAsync()
        {
            var taskListList = new List<List<Task>>();
            var authors = new List<AuthorRoot>();

            var bookRoots = this._bookRootsAccessor.GetBookRoots()
                .Where(AuthorsExist)
                .SelectMany(sm => sm.Authors)
                .DistinctBy(p => p.Key)
                .Select(s => new Tuple<string, string>(s.Key, _authorUrl + s.Key + ".json"))
                .Partition(10)
                .ToList();

            foreach (var source in bookRoots)
            {
                var list = source.Select(s => Task.Run(async () =>
                {
                    try
                    {
                        var authorResponse = await this._httpClient.GetAsync(s.Item2);
                        var data = await authorResponse.Content.ReadAsStringAsync();

                        var authorRoot = JsonConvert.DeserializeObject<AuthorRoot>(data);

                        authorRoot.Name = s.Item1;

                        authors.Add(authorRoot);
                    }
                    catch (Exception ex)
                    {
                        // SWALLOW EXCEPTION
                    }
                })).ToList();

                taskListList.Add(list);
            }

            foreach (var list in taskListList)
            {
                await Task.Delay(TimeSpan.FromSeconds(5));
                await Task.WhenAll(list);
            }

            return this._mapper.Map<List<AuthorRoot>, List<SeedAuthor>>(authors);
        }

        private bool AuthorsExist(BookRoot p)
        {
            return p.Authors != null && p.Authors.Count > 0;
        }
    }
}