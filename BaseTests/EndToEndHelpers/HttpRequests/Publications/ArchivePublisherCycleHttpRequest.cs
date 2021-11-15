using System;
using System.Net.Http;
using BookLovers.Base.Infrastructure;
using FluentHttpRequestBuilderLibrary;
using SimpleE2ETesterLibrary.Interfaces;

namespace BaseTests.EndToEndHelpers.HttpRequests.Publications
{
    public class ArchivePublisherCycleHttpRequest : ISimpleHttpRequest
    {
        private readonly HttpRequestBuilder _builder;

        public ArchivePublisherCycleHttpRequest(Guid publisherCycle, JsonWebToken token)
        {
            _builder = new HttpRequestBuilder()
                .InitializeRequest()
                .WithMethod(HttpMethod.Delete)
                .WithUri($"http://localhost:64892/api/cycles/{publisherCycle}")
                .AddBearerToken(token.AccessToken);
        }

        public HttpRequestMessage ToHttpRequest()
        {
            return _builder.GetRequest();
        }
    }
}