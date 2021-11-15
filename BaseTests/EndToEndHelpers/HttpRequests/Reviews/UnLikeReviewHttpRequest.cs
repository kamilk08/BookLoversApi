using System;
using System.Net.Http;
using BookLovers.Base.Infrastructure;
using FluentHttpRequestBuilderLibrary;
using SimpleE2ETesterLibrary.Interfaces;

namespace BaseTests.EndToEndHelpers.HttpRequests.Reviews
{
    public class UnLikeReviewHttpRequest : ISimpleHttpRequest
    {
        private readonly HttpRequestBuilder _builder;
        private readonly Guid _reviewGuid;

        public UnLikeReviewHttpRequest(Guid reviewGuid, JsonWebToken token)
        {
            _reviewGuid = reviewGuid;
            _builder = new HttpRequestBuilder();

            _builder.InitializeRequest()
                .WithMethod(HttpMethod.Delete)
                .WithUri($"http://localhost:64892/api/reviews/{_reviewGuid}/like")
                .WithStringContent(_reviewGuid)
                .AddBearerToken(token.AccessToken);
        }

        public HttpRequestMessage ToHttpRequest() => _builder.GetRequest();
    }
}