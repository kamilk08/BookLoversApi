using System;
using System.Net.Http;
using AutoFixture;
using BookLovers.Base.Infrastructure;
using BookLovers.Librarians.Application.WriteModels;
using FluentHttpRequestBuilderLibrary;
using SimpleE2ETesterLibrary.Interfaces;

namespace BaseTests.EndToEndHelpers.HttpRequests.Librarians
{
    public class CreateLibrarianHttpRequest : ISimpleHttpRequest
    {
        private readonly HttpRequestBuilder _builder;
        private readonly CreateLibrarianWriteModel _writeModel;

        public Guid LibrarianGuid => _writeModel.LibrarianGuid;

        public CreateLibrarianHttpRequest(JsonWebToken token)
        {
            _writeModel = new Fixture()
                .Build<CreateLibrarianWriteModel>()
                .With(p => p.LibrarianGuid)
                .Without(p => p.ReaderGuid)
                .Create();

            _builder = new HttpRequestBuilder()
                .InitializeRequest()
                .WithMethod(HttpMethod.Post)
                .WithUri("http://localhost:64892/api/librarians")
                .AddBearerToken(token.AccessToken);
        }

        public CreateLibrarianHttpRequest WithReaderGuid(Guid readerGuid)
        {
            _writeModel.ReaderGuid = readerGuid;

            return this;
        }

        public CreateLibrarianHttpRequest WithLibrarianGuid(Guid librarianGuid)
        {
            _writeModel.LibrarianGuid = librarianGuid;

            return this;
        }

        public HttpRequestMessage ToHttpRequest()
        {
            return _builder.WithStringContent(_writeModel).GetRequest();
        }
    }
}