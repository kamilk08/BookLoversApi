using System;
using System.Net.Http;
using AutoFixture;
using BookLovers.Base.Infrastructure;
using BookLovers.Publication.Application.WriteModels.Series;
using FluentHttpRequestBuilderLibrary;
using SimpleE2ETesterLibrary.Interfaces;

namespace BaseTests.EndToEndHelpers.HttpRequests.Publications
{
    public class CreateSeriesHttpRequest : ISimpleHttpRequest
    {
        private readonly HttpRequestBuilder _builder;
        private readonly SeriesWriteModel _writeModel;

        public Guid SeriesGuid => this._writeModel.SeriesGuid;

        public CreateSeriesHttpRequest(JsonWebToken token)
        {
            var fixture = new Fixture();
            this._builder = new HttpRequestBuilder();
            this._writeModel = fixture
                .Build<SeriesWriteModel>()
                .With(p => p.SeriesGuid)
                .With(p => p.SeriesName)
                .Create();

            this._builder.InitializeRequest()
                .WithMethod(HttpMethod.Post)
                .WithUri("http://localhost:64892/api/series")
                .AddBearerToken(token.AccessToken);
        }

        public CreateSeriesHttpRequest WithSeriesGuid(Guid seriesGuid)
        {
            this._writeModel.SeriesGuid = seriesGuid;

            return this;
        }

        public HttpRequestMessage ToHttpRequest()
        {
            return this._builder
                .WithStringContent(this._writeModel)
                .GetRequest();
        }
    }
}