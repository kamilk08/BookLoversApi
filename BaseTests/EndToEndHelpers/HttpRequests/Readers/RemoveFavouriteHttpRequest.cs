using System;
using System.Net.Http;
using AutoFixture;
using BookLovers.Base.Infrastructure;
using BookLovers.Readers.Application.WriteModels.Profiles;
using FluentHttpRequestBuilderLibrary;
using SimpleE2ETesterLibrary.Interfaces;

namespace BaseTests.EndToEndHelpers.HttpRequests.Readers
{
    public class RemoveFavouriteHttpRequest : ISimpleHttpRequest
    {
        private readonly HttpRequestBuilder _builder;
        private readonly Fixture _fixture = new Fixture();

        public RemoveFavouriteHttpRequest(Guid guid, Guid profileGuid, JsonWebToken token)
        {
            var writeModel = _fixture.Build<RemoveFavouriteWriteModel>()
                .With(p => p.FavouriteGuid, guid)
                .With(p => p.ProfileGuid, profileGuid)
                .Create();

            _builder = new HttpRequestBuilder()
                .InitializeRequest()
                .WithMethod(HttpMethod.Delete)
                .WithUri("http://localhost:64892/api/readers/profile/favourites")
                .WithStringContent(writeModel)
                .AddBearerToken(token.AccessToken);
        }

        public HttpRequestMessage ToHttpRequest()
        {
            return _builder.GetRequest();
        }
    }
}