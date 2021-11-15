using System;
using System.Net.Http;
using AutoFixture;
using BookLovers.Base.Infrastructure;
using BookLovers.Publication.Application.WriteModels.Publisher;
using FluentHttpRequestBuilderLibrary;
using SimpleE2ETesterLibrary.Interfaces;

namespace BaseTests.EndToEndHelpers.HttpRequests.Publications
{
    public class CreatePublisherHttpRequest : ISimpleHttpRequest
    {
        private readonly HttpRequestBuilder _builder;
        private readonly AddPublisherWriteModel _writeModel;

        public Guid PublisherGuid => this._writeModel.PublisherGuid;

        public string PublisherName => this._writeModel.Name;

        public CreatePublisherHttpRequest(JsonWebToken token)
        {
            var fixture = new Fixture();
            this._builder = new HttpRequestBuilder();

            this._writeModel = fixture
                .Build<AddPublisherWriteModel>()
                .Without(p => p.PublisherId)
                .With(p => p.PublisherGuid)
                .With(p => p.Name)
                .Create();

            this._builder.InitializeRequest()
                .WithMethod(HttpMethod.Post)
                .WithUri("http://localhost:64892/api/publishers")
                .AddBearerToken(token.AccessToken);
        }

        public CreatePublisherHttpRequest WithPublisherGuid(Guid publisherGuid)
        {
            this._writeModel.PublisherGuid = publisherGuid;

            return this;
        }

        public CreatePublisherHttpRequest WithName(string name)
        {
            this._writeModel.Name = name;

            return this;
        }

        public HttpRequestMessage ToHttpRequest()
        {
            return this._builder.WithStringContent(this._writeModel).GetRequest();
        }
    }
}