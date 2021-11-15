using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookLovers.Seed.Models;

namespace BookLovers.Seed.SeedExecutors
{
    public class SeedExecutionService
    {
        private readonly IDictionary<SeedExecutorType, ISeedExecutor> _seedExecutors;

        public SeedExecutionService(
            IDictionary<SeedExecutorType, ISeedExecutor> seedExecutors)
        {
            _seedExecutors = seedExecutors;
        }

        public async Task SeedAsync(SeedData seed)
        {
            await GetCollectionExecutor<SeedUser>(SeedExecutorType.UsersSeedExecutor)
                .SeedAsync(seed.OwnResourceSeedData.Users);

            await GetCollectionExecutor<SeedSeries>(SeedExecutorType.SeriesSeedExecutor)
                .SeedAsync(seed.OwnResourceSeedData.Series);

            await GetCollectionExecutor<SeedPublisher>(SeedExecutorType.PublisherSeedExecutor)
                .SeedAsync(seed.OpenLibrarySeedData.Publishers);

            await GetCollectionExecutor<SeedAuthor>(SeedExecutorType.AuthorsSeedExecutor)
                .SeedAsync(seed.OpenLibrarySeedData.Authors);

            for (var index = 0; index < seed.OpenLibrarySeedData.Books.Take(30).Count(); ++index)
            {
                var seedSeries = seed.OwnResourceSeedData.Series.ElementAt(0);
                var seedPublisher = seed.OpenLibrarySeedData.Publishers.OrderBy(p => p.Name).ElementAt(0);
                var seedBook = seed.OpenLibrarySeedData.Books.ElementAt(index);

                seedBook.SeriesName = seedSeries.Name;
                seedBook.PositionInSeries = index + 1;
                seedBook.PublisherName = seedPublisher.Name;
            }

            await GetCollectionExecutor<SeedBook>(SeedExecutorType.BooksSeedExecutor)
                .SeedAsync(seed.OpenLibrarySeedData.Books);

            await GetCollectionExecutor<SeedUser>(SeedExecutorType.FollowersExecutor)
                .SeedAsync(seed.OwnResourceSeedData.Users);

            await GetCollectionExecutor<SeedTicket>(SeedExecutorType.UserTicketsExecutor)
                .SeedAsync(seed.OwnResourceSeedData.Tickets);

            await GetCollectionExecutor<SeedQuote>(SeedExecutorType.QuotesSeedExecutor)
                .SeedAsync(seed.OwnResourceSeedData.Quotes);

            await GetSimpleExecutor<Tuple<IEnumerable<SeedUser>, IEnumerable<SeedBook>>>(SeedExecutorType
                    .BookcaseSeedExecutor)
                .SeedAsync(new Tuple<IEnumerable<SeedUser>, IEnumerable<SeedBook>>(
                    seed.OwnResourceSeedData.Users,
                    seed.OpenLibrarySeedData.Books));

            await GetSimpleExecutor<Tuple<IEnumerable<SeedReview>, IEnumerable<SeedBook>>>(SeedExecutorType
                    .ReviewsSeedExecutor)
                .SeedAsync(new Tuple<IEnumerable<SeedReview>, IEnumerable<SeedBook>>(
                    seed.OwnResourceSeedData.Reviews,
                    seed.OpenLibrarySeedData.Books));

            await GetSimpleExecutor<Tuple<IEnumerable<SeedUser>, IEnumerable<SeedBook>>>(SeedExecutorType
                    .RatingsSeedExecutor)
                .SeedAsync(new Tuple<IEnumerable<SeedUser>, IEnumerable<SeedBook>>(
                    seed.OwnResourceSeedData.Users,
                    seed.OpenLibrarySeedData.Books));
        }

        private ICollectionSeedExecutor<T> GetCollectionExecutor<T>(
            SeedExecutorType executorType)
        {
            return _seedExecutors.Values
                .SingleOrDefault(p => p.ExecutorType == executorType) as ICollectionSeedExecutor<T>;
        }

        private ISimpleSeedExecutor<T> GetSimpleExecutor<T>(
            SeedExecutorType executorType)
        {
            return _seedExecutors.Values.SingleOrDefault(
                p => p.ExecutorType == executorType) as ISimpleSeedExecutor<T>;
        }
    }
}