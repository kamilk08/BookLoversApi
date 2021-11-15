using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using AutoFixture;
using BaseTests.DataCreationHelpers;
using BookLovers.Base.Infrastructure;
using BookLovers.Publication.Application.WriteModels.Author;
using BookLovers.Shared.Categories;
using BookLovers.Shared.SharedSexes;
using FluentHttpRequestBuilderLibrary;
using SimpleE2ETesterLibrary.Interfaces;

namespace BaseTests.EndToEndHelpers.HttpRequests.Publications
{
    public class CreateAuthorHttpRequest : ISimpleHttpRequest
    {
        private readonly Guid _readerGuid;
        private readonly Fixture _fixture;
        private readonly HttpRequestBuilder _builder;
        private readonly CreateAuthorWriteModel _writeModel;

        public Guid AuthorGuid => _writeModel.AuthorWriteModel.AuthorGuid;

        public CreateAuthorHttpRequest(Guid addedByGuid, JsonWebToken token)
        {
            _fixture = new Fixture();
            _builder = new HttpRequestBuilder();
            _writeModel = new CreateAuthorWriteModel();

            _readerGuid = addedByGuid;

            _builder.InitializeRequest()
                .WithMethod(HttpMethod.Post)
                .WithUri("http://localhost:64892/api/authors")
                .AddBearerToken(token.AccessToken);
        }

        public CreateAuthorHttpRequest WithAuthorGuid(Guid authorGuid)
        {
            _writeModel.AuthorWriteModel.AuthorGuid = authorGuid;

            return this;
        }

        public CreateAuthorHttpRequest AddCommandData(AuthorWriteModel dto = null)
        {
            _writeModel.AuthorWriteModel = dto ?? CreateDefaultRequestData();

            return this;
        }

        public CreateAuthorHttpRequest WithImage(
            string pathToImage,
            string fileName)
        {
            using (var fileStream = new FileStream(pathToImage, FileMode.Open))
            {
                var bytes = new byte[fileStream.Length];
                fileStream.Write(bytes, 0, bytes.Length);
                _writeModel.PictureWriteModel = new AuthorPictureWriteModel
                {
                    AuthorImage = Convert.ToBase64String(bytes),
                    FileName = fileName
                };
            }

            return this;
        }

        private AuthorWriteModel CreateDefaultRequestData()
        {
            return _fixture.Build<AuthorWriteModel>().WithBooks(new List<Guid>())
                .WithBasics(Sex.Male, _fixture.Create<string>(), _fixture.Create<string>())
                .WithDescription(_fixture.Create<string>(), _fixture.Create<string>(), _fixture.Create<string>())
                .WithDetails(_fixture.Create<DateTime>(), _fixture.Create<string>(), _fixture.Create<DateTime>())
                .WithGenres(
                    new List<int>
                    {
                        SubCategory.FictionSubCategory.Thriller.Value
                    })
                .Without(p => p.AuthorGuid)
                .With(p => p.ReaderGuid, _readerGuid).Create();
        }

        public HttpRequestMessage ToHttpRequest()
        {
            return _builder.WithStringContent(_writeModel).GetRequest();
        }
    }
}