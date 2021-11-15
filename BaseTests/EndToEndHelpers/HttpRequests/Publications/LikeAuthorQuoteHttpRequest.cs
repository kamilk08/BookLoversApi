using System;
using System.Net.Http;
using AutoFixture;
using BookLovers.Base.Infrastructure;
using FluentHttpRequestBuilderLibrary;
using SimpleE2ETesterLibrary.Interfaces;

namespace BaseTests.EndToEndHelpers.HttpRequests.Publications
{
    public class LikeAuthorQuoteHttpRequest : ISimpleHttpRequest
    {
        private readonly HttpRequestBuilder _builder;
        private Fixture _fixture;

        public LikeAuthorQuoteHttpRequest(Guid quoteGuid, JsonWebToken token)
        {
            _builder = new HttpRequestBuilder();
            _fixture = new Fixture();
            _builder
                .InitializeRequest()
                .WithMethod(HttpMethod.Post)
                .WithUri($"http://localhost:64892/api/quotes/{quoteGuid}/like")
                .AddBearerToken(token.AccessToken);
        }

        public HttpRequestMessage ToHttpRequest()
        {
            return _builder.GetRequest();
        }
    }
}