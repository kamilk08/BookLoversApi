using System;
using System.Net.Http;
using AutoFixture;
using BookLovers.Base.Infrastructure;
using BookLovers.Bookcases.Application.WriteModels;
using FluentHttpRequestBuilderLibrary;
using SimpleE2ETesterLibrary.Interfaces;

namespace BaseTests.EndToEndHelpers.HttpRequests.Bookcase
{
    public class RemoveFromShelfRequest : ISimpleHttpRequest
    {
        private readonly HttpRequestBuilder _builder;

        public RemoveFromShelfRequest(
            Guid bookcaseGuid,
            Guid shelfGuid,
            Guid bookGuid,
            JsonWebToken token)
        {
            var writeModel = new Fixture()
                .Build<RemoveFromShelfWriteModel>()
                .With(p => p.BookGuid, bookGuid)
                .With(p => p.BookcaseGuid, bookcaseGuid)
                .With(p => p.ShelfGuid, shelfGuid)
                .Create();

            _builder = new HttpRequestBuilder()
                .InitializeRequest()
                .WithMethod(HttpMethod.Delete)
                .WithUri("http://localhost:64892/api/bookcase/shelves/book")
                .WithStringContent(writeModel)
                .AddBearerToken(token.AccessToken);
        }

        public HttpRequestMessage ToHttpRequest() => _builder.GetRequest();
    }
}