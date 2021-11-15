using System;
using System.Net.Http;
using AutoFixture;
using BookLovers.Base.Infrastructure;
using BookLovers.Ratings.Application.WriteModels;
using BookLovers.Ratings.Domain.RatingStars;
using FluentHttpRequestBuilderLibrary;
using SimpleE2ETesterLibrary.Interfaces;

namespace BaseTests.EndToEndHelpers.HttpRequests.Ratings
{
    public class ChangeRatingHttpRequest : ISimpleHttpRequest
    {
        private readonly HttpRequestBuilder _builder;

        public ChangeRatingHttpRequest(Star star, Guid bookGuid, JsonWebToken token)
        {
            var fixture = new Fixture();
            _builder = new HttpRequestBuilder();

            var writeModel = fixture.Build<ChangeRatingWriteModel>()
                .With(p => p.Stars, star.Value)
                .With(p => p.BookGuid, bookGuid).Create();

            _builder.InitializeRequest()
                .WithMethod(HttpMethod.Put)
                .WithUri("http://localhost:64892/api/ratings")
                .WithStringContent(writeModel)
                .AddBearerToken(token.AccessToken);
        }

        public HttpRequestMessage ToHttpRequest() => _builder.GetRequest();
    }
}