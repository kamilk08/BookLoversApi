using System;
using System.Net.Http;
using AutoFixture;
using BookLovers.Base.Infrastructure;
using BookLovers.Bookcases.Application.WriteModels;
using FluentHttpRequestBuilderLibrary;
using SimpleE2ETesterLibrary.Interfaces;

namespace BaseTests.EndToEndHelpers.HttpRequests.Bookcase
{
    public class RemoveShelfHttpRequest : ISimpleHttpRequest
    {
        private readonly HttpRequestBuilder _builder;

        public RemoveShelfHttpRequest(Guid bookcaseGuid, Guid shelfGuid, JsonWebToken token)
        {
            var writeModel = new Fixture()
                .Build<RemoveShelfWriteModel>()
                .With(p => p.ShelfGuid, shelfGuid)
                .With(p => p.BookcaseGuid, bookcaseGuid)
                .Create();

            _builder = new HttpRequestBuilder()
                .InitializeRequest()
                .WithMethod(HttpMethod.Delete)
                .WithUri("http://localhost:64892/api/bookcase/shelf")
                .WithStringContent(writeModel)
                .AddBearerToken(token.AccessToken);
        }

        public HttpRequestMessage ToHttpRequest() => _builder.GetRequest();
    }
}