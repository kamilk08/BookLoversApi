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
    public class AddRatingHttpRequest : ISimpleHttpRequest
    {
        private readonly HttpRequestBuilder _builder;

        public AddRatingHttpRequest(Star star, Guid bookGuid, JsonWebToken token)
        {
            var fixture = new Fixture();
            _builder = new HttpRequestBuilder();

            var ratingWriteModel = fixture
                .Build<AddRatingWriteModel>()
                .With(p => p.Stars, star.Value)
                .With(p => p.BookGuid, bookGuid).Create();

            _builder.InitializeRequest()
                .WithMethod(HttpMethod.Post)
                .WithUri("http://localhost:64892/api/ratings")
                .WithStringContent(ratingWriteModel)
                .AddBearerToken(token.AccessToken);
        }

        public HttpRequestMessage ToHttpRequest() => _builder.GetRequest();
    }
}