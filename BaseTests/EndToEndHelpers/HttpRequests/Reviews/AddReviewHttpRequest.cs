using System;
using System.Net.Http;
using AutoFixture;
using BookLovers.Base.Infrastructure;
using BookLovers.Readers.Application.WriteModels.Reviews;
using FluentHttpRequestBuilderLibrary;
using SimpleE2ETesterLibrary.Interfaces;

namespace BaseTests.EndToEndHelpers.HttpRequests.Reviews
{
    public class AddReviewHttpRequest : ISimpleHttpRequest
    {
        private readonly HttpRequestBuilder _builder;
        private readonly ReviewWriteModel _writeModel;

        public Guid ReviewGuid => _writeModel.ReviewGuid;

        public AddReviewHttpRequest(Guid reviewGuid, Guid bookGuid, JsonWebToken token)
        {
            _builder = new HttpRequestBuilder();
            var builder = new Fixture();
            _writeModel = builder
                .Build<ReviewWriteModel>()
                .With(p => p.BookGuid, bookGuid)
                .With(p => p.ReviewGuid, reviewGuid)
                .With(p => p.ReviewDetails, new ReviewDetailsWriteModel
                {
                    Content = builder.Create<string>(),
                    EditDate = default(DateTime),
                    MarkedAsSpoiler = false,
                    ReviewDate = builder.Create<DateTime>()
                }).Create();

            _builder
                .InitializeRequest()
                .WithMethod(HttpMethod.Post)
                .WithUri("http://localhost:64892/api/reviews")
                .WithStringContent(_writeModel).AddBearerToken(token.AccessToken);
        }

        public HttpRequestMessage ToHttpRequest() => _builder.GetRequest();
    }
}