using System;
using System.Net.Http;
using AutoFixture;
using BookLovers.Base.Infrastructure;
using BookLovers.Bookcases.Application.WriteModels;
using FluentHttpRequestBuilderLibrary;
using SimpleE2ETesterLibrary.Interfaces;

namespace BaseTests.EndToEndHelpers.HttpRequests.Bookcase
{
    public class ChangeShelfNameRequest : ISimpleHttpRequest
    {
        private readonly HttpRequestBuilder _builder;

        public ChangeShelfNameRequest(
            Guid bookcaseGuid,
            Guid shelfGuid,
            string shelfName,
            JsonWebToken token)
        {
            _builder = new HttpRequestBuilder();

            var writeModel = new Fixture()
                .Build<ChangeShelfNameWriteModel>()
                .With(p => p.BookcaseGuid, bookcaseGuid)
                .With(p => p.ShelfGuid, shelfGuid)
                .With(p => p.ShelfName, shelfName).Create();

            _builder.InitializeRequest()
                .WithMethod(HttpMethod.Put)
                .WithUri("http://localhost:64892/api/bookcase/shelf")
                .WithStringContent(writeModel)
                .AddBearerToken(token.AccessToken);
        }

        public HttpRequestMessage ToHttpRequest() => _builder.GetRequest();
    }
}