using System.Net.Http;
using AutoFixture;
using BookLovers.Auth.Application.WriteModels;
using BookLovers.Base.Infrastructure;
using FluentHttpRequestBuilderLibrary;
using SimpleE2ETesterLibrary.Interfaces;

namespace BaseTests.EndToEndHelpers.HttpRequests.Auth
{
    public class ChangePasswordHttpRequest : ISimpleHttpRequest
    {
        private readonly HttpRequestBuilder _builder;

        public ChangePasswordHttpRequest(
            string currentPassword,
            string nextPassword,
            string email,
            JsonWebToken token)
        {
            var passwordWriteModel = new Fixture()
                .Build<ChangePasswordWriteModel>()
                .With(p => p.Password, currentPassword)
                .With(p => p.Email, email)
                .With(p => p.NewPassword, nextPassword)
                .Create();

            _builder = new HttpRequestBuilder();
            _builder.InitializeRequest()
                .WithMethod(HttpMethod.Put)
                .WithUri("http://localhost:64892/api/auth/password")
                .WithStringContent(passwordWriteModel)
                .AddBearerToken(token.AccessToken);
        }

        public HttpRequestMessage ToHttpRequest()
        {
            return _builder.GetRequest();
        }
    }
}