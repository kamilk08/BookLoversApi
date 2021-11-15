using System;
using System.Net.Http;
using AutoFixture;
using BookLovers.Base.Infrastructure;
using BookLovers.Readers.Application.WriteModels.Reviews;
using FluentHttpRequestBuilderLibrary;
using SimpleE2ETesterLibrary.Interfaces;

namespace BaseTests.EndToEndHelpers.HttpRequests.Reviews
{
    public class EditReviewHttpRequest : ISimpleHttpRequest
    {
        private readonly HttpRequestBuilder _builder;

        public EditReviewHttpRequest(Guid reviewGuid, Guid bookGuid, JsonWebToken token)
        {
            _builder = new HttpRequestBuilder();
            var builder = new Fixture();
            var writeModel = builder
                .Build<ReviewWriteModel>()
                .With(p => p.BookGuid, bookGuid)
                .With(p => p.ReviewGuid, reviewGuid)
                .With(p => p.ReviewDetails, new ReviewDetailsWriteModel
                {
                    Content = builder.Create<string>(),
                    ReviewDate = builder.Create<DateTime>(),
                    EditDate = builder.Create<DateTime>(),
                    MarkedAsSpoiler = false
                }).Create();

            _builder.InitializeRequest()
                .WithMethod(HttpMethod.Put)
                .WithUri("http://localhost:64892/api/reviews")
                .WithStringContent(writeModel)
                .AddBearerToken(token.AccessToken);
        }

        public HttpRequestMessage ToHttpRequest() => _builder.GetRequest();
    }
}