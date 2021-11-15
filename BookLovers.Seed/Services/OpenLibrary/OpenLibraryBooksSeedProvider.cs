using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BookLovers.Seed.Deserializers;
using BookLovers.Seed.Models;
using BookLovers.Seed.Models.Configuration;
using BookLovers.Seed.Models.OpenLibrary.Books;

namespace BookLovers.Seed.Services.OpenLibrary
{
    internal class OpenLibraryBooksSeedProvider :
        IConfigurableSeedProvider<SeedBook, OpenLibrarySeedConfiguration>,
        ISeedProvider<SeedBook>,
        ISeedProvider
    {
        private readonly IMapper _mapper;
        private readonly OpenLibraryBookDeserializer _bookDeserializer;
        private readonly BookRootsAccessor _bookRootsAccessor;
        private OpenLibrarySeedConfiguration _openLibrarySeedConfiguration;

        public SeedProviderType ProviderType => SeedProviderType.Books;

        public SourceType SourceType => SourceType.OpenLibrary;

        public OpenLibraryBooksSeedProvider(
            IMapper mapper,
            OpenLibraryBookDeserializer bookDeserializer,
            BookRootsAccessor bookRootsAccessor)
        {
            this._mapper = mapper;
            this._bookDeserializer = bookDeserializer;
            this._bookRootsAccessor = bookRootsAccessor;
        }

        public Task<IEnumerable<SeedBook>> ProvideAsync()
        {
            var source = this._bookDeserializer
                .Deserialize(this._openLibrarySeedConfiguration);

            this._bookRootsAccessor.SetBookRoots(source);

            var mappedResults = this._mapper.Map<List<BookRoot>, List<SeedBook>>(source).AsEnumerable();

            return Task.FromResult(mappedResults);
        }

        public IConfigurableSeedProvider<SeedBook, OpenLibrarySeedConfiguration> SetSeedConfiguration(
            OpenLibrarySeedConfiguration configuration)
        {
            this._openLibrarySeedConfiguration = configuration;

            return this;
        }
    }
}