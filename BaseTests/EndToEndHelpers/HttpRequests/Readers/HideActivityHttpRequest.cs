using System;
using System.Net.Http;
using AutoFixture;
using BookLovers.Base.Infrastructure;
using BookLovers.Readers.Application.WriteModels.Timelines;
using BookLovers.Readers.Domain.Readers.TimeLineActivities;
using FluentHttpRequestBuilderLibrary;
using SimpleE2ETesterLibrary.Interfaces;

namespace BaseTests.EndToEndHelpers.HttpRequests.Readers
{
    public class HideActivityHttpRequest : ISimpleHttpRequest
    {
        private readonly HttpRequestBuilder _builder;
        private readonly Fixture _fixture = new Fixture();

        public HideActivityHttpRequest(DateTime occuredAt, Guid timeLineObjectGuid, JsonWebToken token)
        {
            var activityWriteModel = _fixture
                .Build<HideTimeLineActivityWriteModel>()
                .With(p => p.OccuredAt, occuredAt)
                .With(p => p.TimeLineObjectGuid, timeLineObjectGuid)
                .With(p => p.ActivityTypeId, ActivityType.NewFollower.Value)
                .Create();

            _builder = new HttpRequestBuilder()
                .InitializeRequest()
                .WithMethod(HttpMethod.Put)
                .WithUri("http://localhost:64892/api/timelines/activity/hide")
                .WithStringContent(activityWriteModel)
                .AddBearerToken(token.AccessToken);
        }

        public HttpRequestMessage ToHttpRequest()
        {
            return _builder.GetRequest();
        }
    }
}