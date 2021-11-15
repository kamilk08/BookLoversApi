using System.Net.Http;
using AutoFixture;
using BookLovers.Base.Infrastructure;
using BookLovers.Publication.Application.WriteModels.Books;
using FluentHttpRequestBuilderLibrary;
using SimpleE2ETesterLibrary.Interfaces;

namespace BaseTests.EndToEndHelpers.HttpRequests.Publications
{
    public class EditBookHttpRequest : ISimpleHttpRequest
    {
        private readonly EditBookWriteModel _writeModel;
        private readonly HttpRequestBuilder _builder;
        private Fixture _fixture;

        public EditBookHttpRequest(JsonWebToken token)
        {
            _builder = new HttpRequestBuilder();
            _fixture = new Fixture();
            _writeModel = new EditBookWriteModel();

            _builder.InitializeRequest()
                .WithMethod(HttpMethod.Put)
                .WithUri("http://localhost:64892/api/books")
                .AddBearerToken(token.AccessToken);
        }

        public EditBookHttpRequest WithBook(BookWriteModel model)
        {
            _writeModel.BookWriteModel = model;

            return this;
        }

        public EditBookHttpRequest WithCover(BookPictureWriteModel writeModel)
        {
            _writeModel.PictureWriteModel = writeModel;

            return this;
        }

        public HttpRequestMessage ToHttpRequest()
        {
            return _builder.WithStringContent(_writeModel).GetRequest();
        }
    }
}