using System;
using System.Collections.Generic;
using System.Net.Http;
using AutoFixture;
using BaseTests.DataCreationHelpers;
using BookLovers.Base.Infrastructure;
using BookLovers.Publication.Application.WriteModels.Books;
using BookLovers.Publication.Domain.Books.CoverTypes;
using BookLovers.Publication.Domain.Books.Languages;
using BookLovers.Shared.Categories;
using FluentHttpRequestBuilderLibrary;
using SimpleE2ETesterLibrary.Interfaces;

namespace BaseTests.EndToEndHelpers.HttpRequests.Publications
{
    public class CreateBookHttpRequest : ISimpleHttpRequest
    {
        private readonly HttpRequestBuilder _builder;
        private readonly BookWriteModel _writeModel;

        public Guid BookGuid => _writeModel.BookGuid;

        public CreateBookHttpRequest(
            Guid publisherGuid,
            Guid seriesGuid,
            Guid userGuid,
            List<Guid> authors,
            JsonWebToken token)
        {
            var builder = new Fixture();
            _builder = new HttpRequestBuilder();

            _writeModel = builder.Build<BookWriteModel>()
                .With(p => p.BookGuid)
                .With(p => p.Description)
                .WithBookBasics(
                    Category.Fiction,
                    SubCategory.FictionSubCategory.Action,
                    "9788375155280",
                    builder.Create<string>(),
                    publisherGuid)
                .WithDetails(builder.Create<int>(), Language.English)
                .WithCover(CoverType.PaperBack, false)
                .WithSeries(seriesGuid, 1)
                .With(p => p.AddedBy, userGuid)
                .With(p => p.Authors, authors)
                .WithCycles(new List<Guid>()).Create();

            _builder.InitializeRequest()
                .WithUri("http://localhost:64892/api/books")
                .WithMethod(HttpMethod.Post)
                .AddBearerToken(token.AccessToken);
        }

        public CreateBookHttpRequest WithBookGuid(Guid bookGuid)
        {
            _writeModel.BookGuid = bookGuid;

            return this;
        }

        public HttpRequestMessage ToHttpRequest()
        {
            return _builder.WithStringContent(GetRequestData()).GetRequest();
        }

        private CreateBookWriteModel GetRequestData()
        {
            return new CreateBookWriteModel
            {
                BookWriteModel = _writeModel,
                PictureWriteModel = new BookPictureWriteModel
                {
                    Cover = string.Empty,
                    FileName = string.Empty
                }
            };
        }
    }
}