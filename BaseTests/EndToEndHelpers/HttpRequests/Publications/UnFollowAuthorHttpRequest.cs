using System;
using System.Net.Http;
using AutoFixture;
using BookLovers.Base.Infrastructure;
using FluentHttpRequestBuilderLibrary;
using SimpleE2ETesterLibrary.Interfaces;

namespace BaseTests.EndToEndHelpers.HttpRequests.Publications
{
    public class UnFollowAuthorHttpRequest : ISimpleHttpRequest
    {
        private readonly HttpRequestBuilder _builder;
        private Fixture _fixture;

        public UnFollowAuthorHttpRequest(Guid authorGuid, JsonWebToken token)
        {
            _builder = new HttpRequestBuilder();
            _fixture = new Fixture();

            _builder.InitializeRequest()
                .WithMethod(HttpMethod.Delete)
                .WithUri($"http://localhost:64892/api/authors/{authorGuid}/follow")
                .AddBearerToken(token.AccessToken);
        }

        public HttpRequestMessage ToHttpRequest()
        {
            return _builder.GetRequest();
        }
    }
}