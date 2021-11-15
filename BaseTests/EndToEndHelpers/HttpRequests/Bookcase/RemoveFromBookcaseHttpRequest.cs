using System;
using System.Net.Http;
using AutoFixture;
using BookLovers.Base.Infrastructure;
using BookLovers.Bookcases.Application.WriteModels;
using FluentHttpRequestBuilderLibrary;
using SimpleE2ETesterLibrary.Interfaces;

namespace BaseTests.EndToEndHelpers.HttpRequests.Bookcase
{
    public class RemoveFromBookcaseHttpRequest : ISimpleHttpRequest
    {
        private readonly HttpRequestBuilder _builder;

        public RemoveFromBookcaseHttpRequest(Guid bookcaseGuid, Guid bookGuid, JsonWebToken token)
        {
            var fixture = new Fixture();
            _builder = new HttpRequestBuilder();

            var writeModel = fixture
                .Build<RemoveFromBookcaseWriteModel>()
                .With(p => p.BookcaseGuid, bookcaseGuid)
                .With(p => p.BookGuid, bookGuid)
                .Create();

            _builder.InitializeRequest()
                .WithMethod(HttpMethod.Delete)
                .WithUri("http://localhost:64892/api/bookcase/book")
                .WithStringContent(writeModel)
                .AddBearerToken(token.AccessToken);
        }

        public HttpRequestMessage ToHttpRequest() => _builder.GetRequest();
    }
}