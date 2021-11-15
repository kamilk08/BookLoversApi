using System;
using System.Net.Http;
using AutoFixture;
using BookLovers.Base.Infrastructure;
using BookLovers.Bookcases.Application.WriteModels;
using FluentHttpRequestBuilderLibrary;
using SimpleE2ETesterLibrary.Interfaces;

namespace BaseTests.EndToEndHelpers.HttpRequests.Bookcase
{
    public class AddToShelfHttpRequest : ISimpleHttpRequest
    {
        private readonly AddBookToBookcaseWriteModel _writeModel;
        private readonly HttpRequestBuilder _builder;

        public Guid ShelfGuid => _writeModel.ShelfGuid;

        public AddToShelfHttpRequest(
            Guid bookcaseGuid,
            Guid shelfGuid,
            Guid bookGuid,
            JsonWebToken token)
        {
            _writeModel = new Fixture()
                .Build<AddBookToBookcaseWriteModel>()
                .With(p => p.BookcaseGuid, bookcaseGuid)
                .With(p => p.BookGuid, bookGuid)
                .With(p => p.ShelfGuid, shelfGuid)
                .Create();

            _builder = new HttpRequestBuilder().InitializeRequest()
                .WithMethod(HttpMethod.Post)
                .WithUri("http://localhost:64892/api/bookcase/book")
                .WithStringContent(_writeModel)
                .AddBearerToken(token.AccessToken);
        }

        public HttpRequestMessage ToHttpRequest() => _builder.GetRequest();
    }
}