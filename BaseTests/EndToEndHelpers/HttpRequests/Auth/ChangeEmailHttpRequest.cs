using System.Net.Http;
using AutoFixture;
using BookLovers.Auth.Application.WriteModels;
using BookLovers.Base.Infrastructure;
using FluentHttpRequestBuilderLibrary;
using SimpleE2ETesterLibrary.Interfaces;

namespace BaseTests.EndToEndHelpers.HttpRequests.Auth
{
    public class ChangeEmailHttpRequest : ISimpleHttpRequest
    {
        private readonly HttpRequestBuilder _builder;

        public ChangeEmailHttpRequest(
            string nextEmail,
            string currentEmail,
            string password,
            JsonWebToken token)
        {
            _builder = new HttpRequestBuilder();

            var changeEmailWriteModel = new Fixture()
                .Build<ChangeEmailWriteModel>()
                .With(p => p.NextEmail, nextEmail)
                .With(p => p.Email, currentEmail)
                .With(p => p.Password, password).Create();

            _builder.InitializeRequest()
                .WithMethod(HttpMethod.Put)
                .WithUri("http://localhost:64892/api/auth/email")
                .WithStringContent(changeEmailWriteModel)
                .AddBearerToken(token.AccessToken);
        }

        public HttpRequestMessage ToHttpRequest()
        {
            return _builder.GetRequest();
        }
    }
}