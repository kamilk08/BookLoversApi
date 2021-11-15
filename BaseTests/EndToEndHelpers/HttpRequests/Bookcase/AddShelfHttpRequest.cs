using System;
using System.Net.Http;
using AutoFixture;
using BookLovers.Base.Infrastructure;
using BookLovers.Bookcases.Application.WriteModels;
using FluentHttpRequestBuilderLibrary;
using SimpleE2ETesterLibrary.Interfaces;

namespace BaseTests.EndToEndHelpers.HttpRequests.Bookcase
{
    public class AddShelfHttpRequest : ISimpleHttpRequest
    {
        private readonly HttpRequestBuilder _builder;
        private readonly AddShelfWriteModel _writeModel;

        public Guid ShelfGuid => _writeModel.ShelfGuid;

        public string ShelfName => _writeModel.ShelfName;

        public AddShelfHttpRequest(Guid bookcaseGuid, JsonWebToken token)
        {
            _writeModel = new Fixture().Build<AddShelfWriteModel>()
                .With(p => p.BookcaseGuid, bookcaseGuid)
                .With(p => p.ShelfGuid).With(p => p.ShelfName)
                .Create();

            _builder = new HttpRequestBuilder()
                .InitializeRequest()
                .WithMethod(HttpMethod.Post)
                .WithUri("http://localhost:64892/api/bookcase/shelf")
                .AddBearerToken(token.AccessToken);
        }

        public AddShelfHttpRequest WithShelfGuid(Guid shelfGuid)
        {
            _writeModel.ShelfGuid = shelfGuid;

            return this;
        }

        public HttpRequestMessage ToHttpRequest() =>
            _builder.WithStringContent(_writeModel).GetRequest();
    }
}