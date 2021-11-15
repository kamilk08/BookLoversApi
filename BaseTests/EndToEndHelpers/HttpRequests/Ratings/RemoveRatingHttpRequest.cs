using System;
using System.Net.Http;
using AutoFixture;
using BookLovers.Base.Infrastructure;
using BookLovers.Ratings.Application.WriteModels;
using FluentHttpRequestBuilderLibrary;
using SimpleE2ETesterLibrary.Interfaces;

namespace BaseTests.EndToEndHelpers.HttpRequests.Ratings
{
    public class RemoveRatingHttpRequest : ISimpleHttpRequest
    {
        private readonly HttpRequestBuilder _builder;

        public RemoveRatingHttpRequest(Guid bookGuid, JsonWebToken token)
        {
            var fixture = new Fixture();
            _builder = new HttpRequestBuilder();

            var ratingWriteModel = fixture.Build<RemoveRatingWriteModel>().With(p => p.BookGuid, bookGuid).Create();

            _builder = new HttpRequestBuilder()
                .InitializeRequest()
                .WithMethod(HttpMethod.Delete)
                .WithUri("http://localhost:64892/api/ratings")
                .WithStringContent(ratingWriteModel)
                .AddBearerToken(token.AccessToken);
        }

        public HttpRequestMessage ToHttpRequest() => _builder.GetRequest();
    }
}