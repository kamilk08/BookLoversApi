using System.Net.Http;
using FluentHttpRequestBuilderLibrary;
using SimpleE2ETesterLibrary.Interfaces;

namespace BaseTests.EndToEndHelpers.HttpRequests.Bookcase
{
    public class GetBookcaseByReaderIdHttpRequest : ISimpleHttpRequest
    {
        private readonly int _readerId;
        private readonly string _token;

        public GetBookcaseByReaderIdHttpRequest(int readerId, string token)
        {
            _readerId = readerId;
            _token = token;
        }

        public HttpRequestMessage ToHttpRequest()
        {
            var request = new HttpRequestBuilder()
                .InitializeRequest()
                .WithMethod(HttpMethod.Get)
                .WithUri($"http://localhost:64892/api/bookcases/reader/{_readerId}")
                .AddBearerToken(_token)
                .GetRequest();

            return request;
        }
    }
}