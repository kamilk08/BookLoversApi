using System;
using System.Collections.Generic;
using System.Net.Http;
using AutoFixture;
using BookLovers.Base.Infrastructure;
using BookLovers.Bookcases.Application.WriteModels;
using BookLovers.Bookcases.Domain.Settings;
using BookLovers.Shared.Privacy;
using FluentHttpRequestBuilderLibrary;
using SimpleE2ETesterLibrary.Interfaces;

namespace BaseTests.EndToEndHelpers.HttpRequests.Bookcase
{
    public class ChangeBookcaseSettingsHttpRequest : ISimpleHttpRequest
    {
        private readonly HttpRequestBuilder _builder;
        private readonly ChangeBookcaseOptionsWriteModel _writeModel;

        private Fixture _fixture;

        public ChangeBookcaseSettingsHttpRequest(Guid bookcaseGuid, JsonWebToken token)
        {
            _fixture = new Fixture();
            _writeModel = new ChangeBookcaseOptionsWriteModel
            {
                BookcaseGuid = bookcaseGuid,
                SelectedOptions = new List<SelectedOptionWriteModel>()
            };

            _builder = new HttpRequestBuilder()
                .InitializeRequest()
                .WithMethod(HttpMethod.Put)
                .WithUri("http://localhost:64892/api/bookcase")
                .AddBearerToken(token.AccessToken);
        }

        public ChangeBookcaseSettingsHttpRequest AddPrivacyOption(
            BookcaseOptionType optionType,
            PrivacyOption privacy)
        {
            _writeModel.SelectedOptions.Add(new SelectedOptionWriteModel
            {
                OptionType = optionType.Value,
                Value = privacy.Value
            });

            return this;
        }

        public HttpRequestMessage ToHttpRequest() =>
            _builder.WithStringContent(_writeModel).GetRequest();
    }
}