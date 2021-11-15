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
    public class ShowTimeLineActivityHttpRequest : ISimpleHttpRequest
    {
        private readonly HttpRequestBuilder _builder;

        public ShowTimeLineActivityHttpRequest(
            Guid timeLineObjectGuid,
            DateTime occuredAt,
            JsonWebToken token)
        {
            _builder = new HttpRequestBuilder();
            var activityWriteModel = new Fixture()
                .Build<ShowTimeLineActivityWriteModel>()
                .With(p => p.OccuredAt, occuredAt)
                .With(p => p.TimeLineObjectGuid, timeLineObjectGuid)
                .With(p => p.ActivityTypeId, ActivityType.NewFollower.Value)
                .Create();

            _builder.InitializeRequest()
                .WithMethod(HttpMethod.Put)
                .WithUri("http://localhost:64892/api/timelines/activity/show")
                .WithStringContent(activityWriteModel)
                .AddBearerToken(token.AccessToken);
        }

        public HttpRequestMessage ToHttpRequest()
        {
            return _builder.GetRequest();
        }
    }
}