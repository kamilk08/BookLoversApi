using System;
using System.Net.Http;
using AutoFixture;
using BookLovers.Base.Infrastructure;
using BookLovers.Bookcases.Application.WriteModels;
using FluentHttpRequestBuilderLibrary;
using SimpleE2ETesterLibrary.Interfaces;

namespace BaseTests.EndToEndHelpers.HttpRequests.Bookcase
{
    public class ChangeShelfHttpRequest : ISimpleHttpRequest
    {
        private readonly HttpRequestBuilder _builder;

        public ChangeShelfHttpRequest(
            Guid bookcaseGuid,
            Guid shelfGuid,
            Guid newShelfGuid,
            Guid bookGuid,
            JsonWebToken token)
        {
            var fixture = new Fixture();
            _builder = new HttpRequestBuilder();

            var writeModel = fixture.Build<ChangeShelfWriteModel>()
                .With(p => p.BookcaseGuid, bookcaseGuid)
                .With(p => p.BookGuid, bookGuid)
                .With(p => p.OldShelfGuid, shelfGuid)
                .With(p => p.NewShelfGuid, newShelfGuid)
                .Create();

            _builder.InitializeRequest()
                .WithMethod(HttpMethod.Put)
                .WithUri("http://localhost:64892/api/bookcase/shelves/book")
                .WithStringContent(writeModel)
                .AddBearerToken(token.AccessToken);
        }

        public HttpRequestMessage ToHttpRequest() => _builder.GetRequest();
    }
}