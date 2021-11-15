using System;
using System.Net.Http;
using System.Threading.Tasks;
using BookLovers.Ratings.Domain;
using FluentHttpRequestBuilderLibrary;
using Newtonsoft.Json;

namespace BookLovers.Ratings.Tests.EndToEndTests
{
    public class FakeBookInBookcaseChecker : IBookInBookcaseChecker
    {
        private readonly HttpClient _httpClient;

        public FakeBookInBookcaseChecker(HttpClient httpClient)
        {
            this._httpClient = httpClient;
        }

        public async Task<bool> IsBookInBookcaseAsync(Guid readerGuid, Guid bookGuid)
        {
            var message = this.CreateMessage(readerGuid, bookGuid);
            var response = await this._httpClient.SendAsync(message);

            string content = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<bool>(content);
        }

        private HttpRequestMessage CreateMessage(Guid readerGuid, Guid bookGuid)
        {
            return new HttpRequestBuilder()
                .InitializeRequest()
                .WithMethod(HttpMethod.Get)
                .WithUri($"http://localhost:64892/api/bookcases/reader/{readerGuid}/book/{bookGuid}").GetRequest();
        }
    }
}