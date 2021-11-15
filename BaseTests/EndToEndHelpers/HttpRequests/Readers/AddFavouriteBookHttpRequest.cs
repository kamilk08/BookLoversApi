using System;
using System.Net.Http;
using AutoFixture;
using BookLovers.Base.Infrastructure;
using BookLovers.Readers.Application.WriteModels.Profiles;
using FluentHttpRequestBuilderLibrary;
using SimpleE2ETesterLibrary.Interfaces;

namespace BaseTests.EndToEndHelpers.HttpRequests.Readers
{
    public class AddFavouriteBookHttpRequest : ISimpleHttpRequest
    {
        private readonly HttpRequestBuilder _builder;
        private readonly AddFavouriteBookWriteModel _writeModel;

        public AddFavouriteBookHttpRequest(Guid bookGuid, Guid profileGuid, JsonWebToken token)
        {
            var fixture = new Fixture();
            _builder = new HttpRequestBuilder();

            _writeModel = fixture
                .Build<AddFavouriteBookWriteModel>()
                .With(p => p.BookGuid, bookGuid)
                .With(p => p.ProfileGuid, profileGuid)
                .With(p => p.AddedAt).Create();

            _builder.InitializeRequest()
                .WithMethod(HttpMethod.Post)
                .WithUri("http://localhost:64892/api/readers/profile/favourites/book")
                .AddBearerToken(token.AccessToken);
        }

        public HttpRequestMessage ToHttpRequest()
        {
            return _builder.WithStringContent(_writeModel).GetRequest();
        }
    }
}