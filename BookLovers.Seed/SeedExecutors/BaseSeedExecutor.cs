using System;
using System.Net.Http;
using System.Threading.Tasks;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Services;
using BookLovers.Readers.Infrastructure.Dtos.Readers;
using FluentHttpRequestBuilderLibrary;

namespace BookLovers.Seed.SeedExecutors
{
    public abstract class BaseSeedExecutor
    {
        protected readonly IAppManager AppManager;
        protected readonly HttpClient HttpClient;
        protected readonly HttpRequestBuilder RequestBuilder;

        protected BaseSeedExecutor(IAppManager appManager)
        {
            AppManager = appManager;
            HttpClient = HttpClientFactory.Create();
            RequestBuilder = new HttpRequestBuilder();
        }

        protected async Task<ReaderDto> GetUserByIdAsync(int userId)
        {
            var response = await HttpClient.GetAsync(UsersUrl(userId));

            return await response.Content.ReadAsAsync<ReaderDto>();
        }

        protected async Task<ReaderDto> GetUserByGuidAsync(Guid userGuid)
        {
            var response = await HttpClient.GetAsync(UsersUrl(userGuid));

            return await response.Content.ReadAsAsync<ReaderDto>();
        }

        protected async Task<JsonWebToken> LoginAsync(string email, string password = "")
        {
            var response = await HttpClient.SendAsync(CreateLoginRequest(email, password));

            return await response.Content.ReadAsAsync<JsonWebToken>();
        }

        protected string UsersUrl(int userId)
        {
            return AppManager.GetConfigValue("users_url") + $"/{userId}";
        }

        protected string UsersUrl(Guid guid)
        {
            return AppManager.GetConfigValue("users_url") + $"/{guid}";
        }

        protected string BookByIdUrl(int bookId)
        {
            return AppManager.GetConfigValue("books_url") + $"/{bookId}";
        }

        protected string BookByGuidUrl(Guid guid)
        {
            return AppManager.GetConfigValue("books_url") + $"/{guid}";
        }

        private HttpRequestMessage CreateLoginRequest(string email, string password = "")
        {
            return RequestBuilder
                .InitializeRequest()
                .WithMethod(HttpMethod.Post)
                .WithUri(AppManager.GetConfigValue("login"))
                .WithFormUrlEncodedContent()
                .AddFormKeyValue("username", email)
                .AddFormKeyValue(nameof(password), password == string.Empty ? "Babcia123!" : password)
                .AddFormKeyValue("grant_type", nameof(password))
                .AddFormKeyValue("client_id", "00f80a32-0205-4aff-94d9-46635d8c431c")
                .AddFormKeyValue("client_secret", "DUPA")
                .PassFormValuesAsHttpContent()
                .AddHeader("Cache-Control", "no-cache")
                .AddHeader("Connection", "keep-alive")
                .GetRequest();
        }
    }
}