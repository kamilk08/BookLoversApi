using System;
using System.Net.Http;
using AutoFixture;
using BookLovers.Auth.Application.WriteModels;
using FluentHttpRequestBuilderLibrary;
using SimpleE2ETesterLibrary.Interfaces;

namespace BaseTests.EndToEndHelpers.HttpRequests.Auth
{
    public class RegisterUserHttpRequest : ISimpleHttpRequest
    {
        private readonly Fixture _fixture;
        private readonly SignUpWriteModel _signUpWriteModel;
        private readonly HttpRequestBuilder _builder;

        public Guid BookcaseGuid => _signUpWriteModel.BookcaseGuid;

        public Guid ProfileGuid => _signUpWriteModel.ProfileGuid;

        public Guid UserGuid => _signUpWriteModel.UserGuid;

        public RegisterUserHttpRequest()
        {
            _fixture = new Fixture();
            _builder = new HttpRequestBuilder();

            _signUpWriteModel = _fixture.Build<SignUpWriteModel>()
                .With(p => p.BookcaseGuid).With(p => p.ProfileGuid)
                .Create();

            _builder.InitializeRequest()
                .WithMethod(HttpMethod.Post)
                .WithUri("http://localhost:64892/api/auth/sign_up");
        }

        public RegisterUserHttpRequest WithUserGuid(Guid userGuid)
        {
            _signUpWriteModel.UserGuid = userGuid;

            return this;
        }

        public RegisterUserHttpRequest WithProfileGuid(Guid profileGuid)
        {
            _signUpWriteModel.ProfileGuid = profileGuid;

            return this;
        }

        public RegisterUserHttpRequest WithBookcaseGuid(Guid bookcaseGuid)
        {
            _signUpWriteModel.BookcaseGuid = bookcaseGuid;

            return this;
        }

        public RegisterUserHttpRequest WithEmailAndUserName(
            string email,
            string username)
        {
            _signUpWriteModel.Account.AccountDetails = _fixture
                .Build<AccountDetailsWriteModel>()
                .With(p => p.Email, email)
                .With(p => p.UserName, username)
                .Create();

            return this;
        }

        public RegisterUserHttpRequest WithPassword(string password)
        {
            _signUpWriteModel.Account.AccountSecurity =
                _fixture.Build<AccountSecurityWriteModel>()
                    .With(p => p.Password, password)
                    .Create();

            return this;
        }

        public HttpRequestMessage ToHttpRequest()
        {
            return _builder.WithStringContent(_signUpWriteModel).GetRequest();
        }
    }
}