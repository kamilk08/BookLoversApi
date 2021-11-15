using System;
using System.Net.Http;
using AutoFixture;
using BookLovers.Auth.Application.WriteModels;
using BookLovers.Base.Infrastructure;
using FluentHttpRequestBuilderLibrary;
using SimpleE2ETesterLibrary.Interfaces;

namespace BaseTests.EndToEndHelpers.HttpRequests.Auth
{
    public class RevokeTokenHttpRequest : ISimpleHttpRequest
    {
        private readonly HttpRequestBuilder _builder;

        public RevokeTokenHttpRequest(Guid tokenGuid, JsonWebToken token)
        {
            var revokeTokenWriteModel =
                new Fixture().Build<RevokeTokenWriteModel>()
                    .With(p => p.TokenGuid, tokenGuid).Create();

            _builder = new HttpRequestBuilder()
                .InitializeRequest()
                .WithMethod(HttpMethod.Delete)
                .WithUri("http://localhost:64892/api/auth/token")
                .WithStringContent(revokeTokenWriteModel)
                .AddBearerToken(token.AccessToken);
        }

        public HttpRequestMessage ToHttpRequest()
        {
            return _builder.GetRequest();
        }
    }
}