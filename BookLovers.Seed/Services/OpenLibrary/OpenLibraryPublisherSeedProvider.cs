using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookLovers.Base.Infrastructure.Extensions;
using BookLovers.Seed.Models;
using BookLovers.Seed.Models.OpenLibrary.Books;

namespace BookLovers.Seed.Services.OpenLibrary
{
    internal class OpenLibraryPublisherSeedProvider : ISeedProvider<SeedPublisher>, ISeedProvider
    {
        private readonly BookRootsAccessor _bookRootsAccessor;

        public SeedProviderType ProviderType => SeedProviderType.Publisher;

        public SourceType SourceType => SourceType.OpenLibrary;

        public OpenLibraryPublisherSeedProvider(BookRootsAccessor bookRootsAccessor)
        {
            this._bookRootsAccessor = bookRootsAccessor;
        }

        public Task<IEnumerable<SeedPublisher>> ProvideAsync()
        {
            var publishers = this._bookRootsAccessor.GetBookRoots()
                .Where(this.PublisherMustExist)
                .SelectMany(sm => sm.Publishers)
                .DistinctBy(p => p).Select(s => new SeedPublisher(s));

            return Task.FromResult(publishers);
        }

        private bool PublisherMustExist(BookRoot p) => p.Publishers != null;
    }
}