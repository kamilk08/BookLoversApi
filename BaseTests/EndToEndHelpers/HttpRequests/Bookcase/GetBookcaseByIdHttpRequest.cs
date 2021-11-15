using System.Net.Http;
using FluentHttpRequestBuilderLibrary;
using SimpleE2ETesterLibrary.Interfaces;

namespace BaseTests.EndToEndHelpers.HttpRequests.Bookcase
{
    public class GetBookcaseByIdHttpRequest : ISimpleHttpRequest
    {
        private readonly int _bookcaseId;
        private readonly string _token;

        public GetBookcaseByIdHttpRequest(int bookcaseId, string token)
        {
            _bookcaseId = bookcaseId;
            _token = token;
        }

        public HttpRequestMessage ToHttpRequest()
        {
            var request = new HttpRequestBuilder()
                .InitializeRequest()
                .WithMethod(HttpMethod.Get)
                .WithUri($@"http://localhost:64892/api/bookcases/{_bookcaseId}")
                .AddBearerToken(_token)
                .GetRequest();

            return request;
        }
    }
}