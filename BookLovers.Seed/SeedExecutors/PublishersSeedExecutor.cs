using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Services;
using BookLovers.Publication.Application.WriteModels.Publisher;
using BookLovers.Seed.Models;
using FluentHttpRequestBuilderLibrary;

namespace BookLovers.Seed.SeedExecutors
{
    public class PublishersSeedExecutor : BaseSeedExecutor,
        ICollectionSeedExecutor<SeedPublisher>,
        ISeedExecutor
    {
        private const int UserId = 1;

        private JsonWebToken _token;

        public SeedExecutorType ExecutorType => SeedExecutorType.PublisherSeedExecutor;

        public PublishersSeedExecutor(IAppManager appManager)
            : base(appManager)
        {
        }

        public async Task SeedAsync(IEnumerable<SeedPublisher> seed)
        {
            var userByIdAsync = await this.GetUserByIdAsync(UserId);
            this._token = await this.LoginAsync(userByIdAsync.Email);

            var seedPublishers = seed.OrderBy(p => p.Name).ToList();

            foreach (var seedPublisher in seedPublishers)
            {
                var publisherRequest = this.CreatePublisherRequest(seedPublisher);
                var response = await this.HttpClient.SendAsync(publisherRequest);
            }
        }

        private HttpRequestMessage CreatePublisherRequest(SeedPublisher seedPublisher)
        {
            var publisherWriteModel = new AddPublisherWriteModel
            {
                Name = seedPublisher.Name,
                PublisherGuid = Guid.NewGuid()
            };

            return RequestBuilder.InitializeRequest()
                .WithMethod(HttpMethod.Post)
                .WithUri(CreatePublisherUrl())
                .WithStringContent(publisherWriteModel)
                .AddBearerToken(_token.AccessToken).GetRequest();
        }

        private string CreatePublisherUrl()
        {
            return AppManager.GetConfigValue("publishers_url");
        }
    }
}