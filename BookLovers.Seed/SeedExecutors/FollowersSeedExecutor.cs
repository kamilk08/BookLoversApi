using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using BookLovers.Base.Infrastructure.Services;
using BookLovers.Readers.Application.WriteModels.Readers;
using BookLovers.Seed.Models;
using FluentHttpRequestBuilderLibrary;

namespace BookLovers.Seed.SeedExecutors
{
    public class FollowersSeedExecutor :
        BaseSeedExecutor,
        ICollectionSeedExecutor<SeedUser>,
        ISeedExecutor
    {
        private List<SeedUser> _seedUsers;

        public SeedExecutorType ExecutorType => SeedExecutorType.FollowersExecutor;

        public FollowersSeedExecutor(IAppManager appManager)
            : base(appManager)
        {
        }

        public async Task SeedAsync(IEnumerable<SeedUser> seed)
        {
            this._seedUsers = seed.ToList();
            var firstUser = this._seedUsers.First();
            var usersToFollow = this._seedUsers.Skip(1).ToList();
            var token = await this.LoginAsync(firstUser.Email, firstUser.Password);

            foreach (var seedUser in usersToFollow)
            {
                var response = await this.FollowUserAsync(seedUser.UserGuid, token.AccessToken);
            }

            foreach (var seedUser in usersToFollow)
            {
                token = await this.LoginAsync(seedUser.Email, seedUser.Password);

                var response = await this.FollowUserAsync(firstUser.UserGuid, token.AccessToken);
            }
        }

        private Task<HttpResponseMessage> FollowUserAsync(
            Guid followedGuid,
            string token)
        {
            var writeModel = new ReaderFollowWriteModel
            {
                FollowedGuid = followedGuid
            };

            var request = RequestBuilder.InitializeRequest()
                .WithMethod(HttpMethod.Post)
                .WithUri(AppManager.GetConfigValue("add_or_remove_follower"))
                .WithStringContent(writeModel)
                .AddHeader("Cache-Control", "no-cache")
                .AddHeader("Connection", "keep-alive")
                .AddBearerToken(token)
                .GetRequest();

            return HttpClient.SendAsync(request);
        }
    }
}