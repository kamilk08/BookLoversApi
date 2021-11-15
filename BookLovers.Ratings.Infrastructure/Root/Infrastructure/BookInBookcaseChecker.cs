using System;
using System.Net.Http;
using System.Threading.Tasks;
using BookLovers.Ratings.Domain;
using FluentHttpRequestBuilderLibrary;

namespace BookLovers.Ratings.Infrastructure.Root.Infrastructure
{
    internal class BookInBookcaseChecker : IBookInBookcaseChecker
    {
        private readonly HttpClient _httpClient;

        public BookInBookcaseChecker()
        {
            _httpClient = HttpClientFactory.Create();
        }

        public async Task<bool> IsBookInBookcaseAsync(Guid readerGuid, Guid bookGuid)
        {
            var response = await _httpClient.SendAsync(CreateMessage(readerGuid, bookGuid));

            return await response.Content.ReadAsAsync<bool>();
        }

        private HttpRequestMessage CreateMessage(Guid readerGuid, Guid bookGuid)
        {
            return new HttpRequestBuilder()
                .InitializeRequest()
                .WithMethod(HttpMethod.Get)
                .WithUri($"http://localhost:64892/api/bookcases/reader/{readerGuid}/book/{bookGuid}")
                .GetRequest();
        }
    }
}