using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using BookLovers.Base.Infrastructure.Extensions;
using BookLovers.Base.Infrastructure.Services;
using BookLovers.Ratings.Application.WriteModels;
using BookLovers.Seed.Models;
using FluentHttpRequestBuilderLibrary;

namespace BookLovers.Seed.SeedExecutors
{
    internal class RatingsSeedExecutor :
        BaseSeedExecutor,
        ISimpleSeedExecutor<Tuple<IEnumerable<SeedUser>, IEnumerable<SeedBook>>>,
        ISeedExecutor
    {
        private const int OneStar = 1;
        private const int TwoStars = 2;
        private const int ThreeStars = 3;
        private const int FourStars = 4;
        private const int FiveStars = 5;

        public SeedExecutorType ExecutorType => SeedExecutorType.RatingsSeedExecutor;

        public RatingsSeedExecutor(IAppManager appManager)
            : base(appManager)
        {
        }

        public async Task SeedAsync(
            Tuple<IEnumerable<SeedUser>, IEnumerable<SeedBook>> seed)
        {
            var seedUsers = seed.Item1.ToList();
            var seedBooks = seed.Item2.Skip(1).Take(15).ToList().Partition(5).ToList();

            var booksWithFiveStars = seedBooks.ElementAt(0).ToList();
            var booksWithFourStars = seedBooks.ElementAt(1).ToList();
            var booksWithThreeStars = seedBooks.ElementAt(2).ToList();
            var booksWithTwoStars = seedBooks.ElementAt(3).ToList();
            var booksWithOneStars = seedBooks.ElementAt(4).ToList();

            foreach (var seedUser in seedUsers)
            {
                var token = await this.LoginAsync(seedUser.Email, seedUser.Password);

                foreach (var seedBook in booksWithFiveStars)
                    await this.AddRatingAsync(seedBook.BookGuid, seedUser.UserGuid, FiveStars, token.AccessToken);

                foreach (var seedBook in booksWithFourStars)
                    await this.AddRatingAsync(seedBook.BookGuid, seedUser.UserGuid, FourStars, token.AccessToken);

                foreach (var seedBook in booksWithThreeStars)
                    await this.AddRatingAsync(seedBook.BookGuid, seedUser.UserGuid, ThreeStars, token.AccessToken);

                foreach (var seedBook in booksWithTwoStars)
                    await this.AddRatingAsync(seedBook.BookGuid, seedUser.UserGuid, TwoStars, token.AccessToken);

                foreach (var seedBook in booksWithOneStars)
                    await this.AddRatingAsync(seedBook.BookGuid, seedUser.UserGuid, OneStar, token.AccessToken);
            }
        }

        private async Task AddRatingAsync(Guid bookGuid, Guid readerGuid, int stars, string token)
        {
            var isInBookcaseRequest = this.RequestBuilder
                .InitializeRequest()
                .WithMethod(HttpMethod.Get)
                .WithUri($"http://localhost:64892/api/bookcases/reader/{readerGuid}/book/{bookGuid}")
                .GetRequest();

            var isInBookcaseResponse = await this.HttpClient.SendAsync(isInBookcaseRequest);

            if (!await isInBookcaseResponse.Content.ReadAsAsync<bool>())
                return;

            var ratingWriteModel = new AddRatingWriteModel
            {
                Stars = stars,
                BookGuid = bookGuid
            };

            var addRatingRequest = this.RequestBuilder.InitializeRequest()
                .WithMethod(HttpMethod.Post)
                .WithUri(this.AddRatingUrl())
                .WithStringContent(ratingWriteModel)
                .AddBearerToken(token).GetRequest();

            var response = await this.HttpClient.SendAsync(addRatingRequest);
        }

        private string AddRatingUrl()
        {
            return AppManager.GetConfigValue("add_rating_url");
        }
    }
}