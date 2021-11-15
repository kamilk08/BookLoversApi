using System;
using System.Net.Http;
using AutoFixture;
using BookLovers.Base.Infrastructure;
using BookLovers.Readers.Application.WriteModels.Profiles;
using BookLovers.Shared.SharedSexes;
using FluentHttpRequestBuilderLibrary;
using SimpleE2ETesterLibrary.Interfaces;

namespace BaseTests.EndToEndHelpers.HttpRequests.Readers
{
    public class EditProfileHttpRequest : ISimpleHttpRequest
    {
        private readonly HttpRequestBuilder _builder;

        public EditProfileHttpRequest(Guid profileGuid, JsonWebToken token)
        {
            _builder = new HttpRequestBuilder();
            var builder = new Fixture();
            var writeModel = builder
                .Build<ProfileWriteModel>()
                .With(p => p.ProfileGuid, profileGuid)
                .With(p => p.About)
                .With(p => p.Address)
                .With(p => p.Identity, new IdentityWriteModel
                {
                    BirthDate = builder.Create<DateTime>(),
                    FirstName = builder.Create<string>(),
                    SecondName = builder.Create<string>(),
                    Sex = Sex.Male.Value
                }).Create();

            _builder
                .InitializeRequest()
                .WithMethod(HttpMethod.Put)
                .WithUri("http://localhost:64892/api/readers/profile")
                .WithStringContent(writeModel)
                .AddBearerToken(token.AccessToken);
        }

        public HttpRequestMessage ToHttpRequest()
        {
            return _builder.GetRequest();
        }
    }
}