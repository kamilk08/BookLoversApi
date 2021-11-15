using System;
using System.Net.Http;
using AutoFixture;
using BookLovers.Base.Infrastructure;
using BookLovers.Librarians.Domain.ReviewReportRegisters;
using BookLovers.Readers.Application.WriteModels.Reviews;
using FluentHttpRequestBuilderLibrary;
using SimpleE2ETesterLibrary.Interfaces;

namespace BaseTests.EndToEndHelpers.HttpRequests.Reviews
{
    public class ReportReviewHttpRequest : ISimpleHttpRequest
    {
        private readonly HttpRequestBuilder _builder;
        private readonly Fixture _fixture = new Fixture();

        public ReportReviewHttpRequest(Guid reviewGuid, ReportReason reportReason, JsonWebToken token)
        {
            var writeModel = _fixture.Build<ReportReviewWriteModel>()
                .With(p => p.ReviewGuid, reviewGuid)
                .With(p => p.ReportReasonId, reportReason.Value)
                .Create();

            _builder = new HttpRequestBuilder()
                .InitializeRequest()
                .WithMethod(HttpMethod.Post)
                .WithUri("http://localhost:64892/api/reviews/report")
                .WithStringContent(writeModel)
                .AddBearerToken(token.AccessToken);
        }

        public HttpRequestMessage ToHttpRequest() => _builder.GetRequest();
    }
}