using System;
using System.Net.Http;
using AutoFixture;
using BookLovers.Auth.Application.WriteModels;
using BookLovers.Base.Infrastructure;
using FluentHttpRequestBuilderLibrary;
using SimpleE2ETesterLibrary.Interfaces;

namespace BaseTests.EndToEndHelpers.HttpRequests.Auth
{
    public class BlockAccountHttpRequest : ISimpleHttpRequest
    {
        private readonly HttpRequestBuilder _builder;

        public BlockAccountHttpRequest(Guid userToBlockGuid, JsonWebToken token)
        {
            var accountWriteModel = new Fixture()
                .Build<BlockAccountWriteModel>()
                .With(p => p.BlockedReaderGuid, userToBlockGuid)
                .Create();

            _builder = new HttpRequestBuilder();

            _builder.InitializeRequest()
                .WithMethod(HttpMethod.Delete)
                .WithUri("http://localhost:64892/api/auth/user")
                .WithStringContent(accountWriteModel)
                .AddBearerToken(token.AccessToken);
        }

        public HttpRequestMessage ToHttpRequest()
        {
            return _builder.GetRequest();
        }
    }
}