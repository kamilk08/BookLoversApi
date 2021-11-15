using System;
using System.Net.Http;
using BookLovers.Base.Infrastructure;
using FluentHttpRequestBuilderLibrary;
using SimpleE2ETesterLibrary.Interfaces;

namespace BaseTests.EndToEndHelpers.HttpRequests.Librarians
{
    public class DegradeLibrarianHttpRequest : ISimpleHttpRequest
    {
        private readonly HttpRequestBuilder _builder;

        public DegradeLibrarianHttpRequest(Guid userGuid, JsonWebToken token)
        {
            _builder = new HttpRequestBuilder()
                .InitializeRequest()
                .WithMethod(HttpMethod.Delete)
                .WithUri($"http://localhost:64892/api/librarians/{userGuid}")
                .AddBearerToken(token.AccessToken);
        }

        public HttpRequestMessage ToHttpRequest()
        {
            return _builder.GetRequest();
        }
    }
}