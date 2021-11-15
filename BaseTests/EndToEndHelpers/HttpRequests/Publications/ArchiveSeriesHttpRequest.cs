using System;
using System.Net.Http;
using BookLovers.Base.Infrastructure;
using FluentHttpRequestBuilderLibrary;
using SimpleE2ETesterLibrary.Interfaces;

namespace BaseTests.EndToEndHelpers.HttpRequests.Publications
{
    public class ArchiveSeriesHttpRequest : ISimpleHttpRequest
    {
        private readonly HttpRequestBuilder _builder;

        public ArchiveSeriesHttpRequest(Guid seriesGuid, JsonWebToken token)
        {
            _builder = new HttpRequestBuilder()
                .InitializeRequest()
                .WithMethod(HttpMethod.Delete)
                .WithUri($"http://localhost:64892/api/series/{seriesGuid}")
                .AddBearerToken(token.AccessToken);
        }

        public HttpRequestMessage ToHttpRequest()
        {
            return _builder.GetRequest();
        }
    }
}