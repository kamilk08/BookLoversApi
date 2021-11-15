using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using BookLovers.Base.Infrastructure.Services;
using BookLovers.Readers.Application.WriteModels.Reviews;
using BookLovers.Seed.Models;
using FluentHttpRequestBuilderLibrary;

namespace BookLovers.Seed.SeedExecutors
{
    internal class ReviewsSeedExecutor :
        BaseSeedExecutor,
        ISimpleSeedExecutor<Tuple<IEnumerable<SeedReview>, IEnumerable<SeedBook>>>,
        ISeedExecutor
    {
        public SeedExecutorType ExecutorType => SeedExecutorType.ReviewsSeedExecutor;

        public ReviewsSeedExecutor(IAppManager appManager)
            : base(appManager)
        {
        }

        public async Task SeedAsync(Tuple<IEnumerable<SeedReview>, IEnumerable<SeedBook>> seed)
        {
            var reviews = seed.Item1.ToList();
            foreach (var externalBook in seed.Item2.Skip(1).Take(15).ToList())
            {
                foreach (var seedReview in reviews)
                {
                    var jsonWebToken = await this.LoginAsync(seedReview.SeedUser.Email, seedReview.SeedUser.Password);
                    seedReview.ReviewGuid = Guid.NewGuid();

                    var reviewResponse = await this.CreateReviewAsync(seedReview, externalBook.BookGuid,
                        jsonWebToken.AccessToken);
                }
            }
        }

        private Task<HttpResponseMessage> CreateReviewAsync(
            SeedReview seedReview,
            Guid bookGuid,
            string token)
        {
            var reviewWriteModel = new ReviewWriteModel
            {
                ReviewGuid = seedReview.ReviewGuid,
                BookGuid = bookGuid,
                ReviewDetails = new ReviewDetailsWriteModel
                {
                    Content = seedReview.Content,
                    MarkedAsSpoiler = false,
                    EditDate = default(DateTime),
                    ReviewDate = seedReview.ReviewDate
                }
            };

            var request = RequestBuilder
                .InitializeRequest()
                .WithMethod(HttpMethod.Post)
                .WithUri(AddReviewUrl())
                .AddBearerToken(token)
                .WithStringContent(reviewWriteModel)
                .GetRequest();

            return HttpClient.SendAsync(request);
        }

        private string AddReviewUrl()
        {
            return AppManager.GetConfigValue("reviews_url");
        }
    }
}