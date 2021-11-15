using System;
using System.Net.Http;
using AutoFixture;
using BookLovers.Base.Infrastructure;
using BookLovers.Publication.Application.WriteModels.PublisherCycles;
using FluentHttpRequestBuilderLibrary;
using SimpleE2ETesterLibrary.Interfaces;

namespace BaseTests.EndToEndHelpers.HttpRequests.Publications
{
    public class CreatePublisherCycleHttpRequest : ISimpleHttpRequest
    {
        private readonly HttpRequestBuilder _builder;
        private readonly AddCycleWriteModel _writeModel;

        public CreatePublisherCycleHttpRequest(Guid publisherGuid, JsonWebToken token)
        {
            var fixture = new Fixture();
            this._builder = new HttpRequestBuilder();
            this._writeModel = fixture
                .Build<AddCycleWriteModel>()
                .With(p => p.Cycle)
                .With(p => p.CycleGuid)
                .With(p => p.PublisherGuid, publisherGuid)
                .Create();

            this._builder.InitializeRequest()
                .WithMethod(HttpMethod.Post)
                .WithUri("http://localhost:64892/api/cycles")
                .AddBearerToken(token.AccessToken);
        }

        public CreatePublisherCycleHttpRequest WithCycleGuid(Guid guid)
        {
            this._writeModel.CycleGuid = guid;

            return this;
        }

        public HttpRequestMessage ToHttpRequest()
        {
            return this._builder.WithStringContent(this._writeModel)
                .GetRequest();
        }
    }
}