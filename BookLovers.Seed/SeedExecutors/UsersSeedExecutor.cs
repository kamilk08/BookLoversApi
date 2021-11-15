using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using BookLovers.Auth.Application.WriteModels;
using BookLovers.Base.Infrastructure.Services;
using BookLovers.Seed.Models;
using FluentHttpRequestBuilderLibrary;

namespace BookLovers.Seed.SeedExecutors
{
    internal class UsersSeedExecutor :
        BaseSeedExecutor,
        ICollectionSeedExecutor<SeedUser>,
        ISeedExecutor
    {
        public UsersSeedExecutor(IAppManager appManager)
            : base(appManager)
        {
        }

        public SeedExecutorType ExecutorType => SeedExecutorType.UsersSeedExecutor;

        public async Task SeedAsync(IEnumerable<SeedUser> seed)
        {
            var seedUsers = seed.ToList();

            foreach (var seedUser in seedUsers)
            {
                var response = await CreateUserAsync(seedUser);
            }

            foreach (var seedUser in seedUsers)
            {
                var registrationToken = await GetRegistrationTokenAsync(seedUser);
                var response = await CompleteRegistrationAsync(seedUser.Email, registrationToken);
            }
        }

        private Task<HttpResponseMessage> CreateUserAsync(SeedUser seedUser)
        {
            var signUpWriteModel = new SignUpWriteModel
            {
                BookcaseGuid = seedUser.BookcaseGuid,
                UserGuid = seedUser.UserGuid,
                ProfileGuid = Guid.NewGuid(),
                Account = new AccountWriteModel
                {
                    AccountDetails = new AccountDetailsWriteModel
                    {
                        Email = seedUser.Email,
                        UserName = seedUser.UserName
                    },
                    AccountSecurity = new AccountSecurityWriteModel
                    {
                        Password = seedUser.Password
                    }
                }
            };

            var request = RequestBuilder
                .InitializeRequest()
                .WithMethod(HttpMethod.Post)
                .WithUri(AppManager.GetConfigValue("sign_up_url"))
                .WithStringContent(signUpWriteModel)
                .GetRequest();

            return HttpClient.SendAsync(request);
        }

        private Task<HttpResponseMessage> CompleteRegistrationAsync(
            string email,
            string token)
        {
            var request = new HttpRequestBuilder().InitializeRequest().WithMethod(HttpMethod.Put)
                .WithUri(CompleteRegistrationUrl(email, token)).GetRequest();

            return HttpClient.SendAsync(request);
        }

        private async Task<string> GetRegistrationTokenAsync(SeedUser seedUser)
        {
            var request = new HttpRequestBuilder()
                .InitializeRequest()
                .WithMethod(HttpMethod.Get)
                .WithUri(GetRegistrationTokenUrl(seedUser.Email)).GetRequest();

            var response = await this.HttpClient.SendAsync(request);

            return await response.Content.ReadAsAsync<string>();
        }

        private string CompleteRegistrationUrl(string email, string token) =>
            AppManager.GetConfigValue("registration_url") + "/" + email + "/" + token;

        private string GetRegistrationTokenUrl(string email) =>
            AppManager.GetConfigValue("registration_url") + "/" + email + "/";
    }
}