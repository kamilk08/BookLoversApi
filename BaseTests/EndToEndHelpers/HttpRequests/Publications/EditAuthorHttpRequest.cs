using System.Net.Http;
using AutoFixture;
using BookLovers.Base.Infrastructure;
using BookLovers.Publication.Application.WriteModels.Author;
using BookLovers.Publication.Application.WriteModels.Books;
using FluentHttpRequestBuilderLibrary;
using SimpleE2ETesterLibrary.Interfaces;

namespace BaseTests.EndToEndHelpers.HttpRequests.Publications
{
    public class EditAuthorHttpRequest : ISimpleHttpRequest
    {
        private readonly EditAuthorWriteModel _writeModel;
        private readonly HttpRequestBuilder _builder;

        private Fixture _fixture;

        public EditAuthorHttpRequest(EditAuthorWriteModel writeModel, JsonWebToken token)
        {
            _fixture = new Fixture();
            _builder = new HttpRequestBuilder();
            _writeModel = writeModel;

            _builder.InitializeRequest()
                .WithMethod(HttpMethod.Put)
                .WithUri("http://localhost:64892/api/authors")
                .AddBearerToken(token.AccessToken);
        }

        public EditAuthorHttpRequest WithAuthor(AuthorWriteModel dto)
        {
            _writeModel.AuthorWriteModel = dto;

            return this;
        }

        public EditAuthorHttpRequest WithPicture(
            AuthorPictureWriteModel pictureWriteModel)
        {
            _writeModel.PictureWriteModel = pictureWriteModel;

            return this;
        }

        public HttpRequestMessage ToHttpRequest()
        {
            return _builder.WithStringContent(_writeModel).GetRequest();
        }
    }
}