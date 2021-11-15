using System;
using System.Net.Http;
using AutoFixture;
using BookLovers.Base.Infrastructure;
using BookLovers.Publication.Application.WriteModels.Quotes;
using FluentHttpRequestBuilderLibrary;
using SimpleE2ETesterLibrary.Interfaces;

namespace BaseTests.EndToEndHelpers.HttpRequests.Publications
{
    public class CreateAuthorQuoteHttpRequest : ISimpleHttpRequest
    {
        private readonly HttpRequestBuilder _builder;
        private readonly AddQuoteWriteModel _writeModel;

        public CreateAuthorQuoteHttpRequest(Guid authorGuid, JsonWebToken token)
        {
            _builder = new HttpRequestBuilder();
            _writeModel = new Fixture().Build<AddQuoteWriteModel>()
                .With(p => p.Quote)
                .With(p => p.AddedAt)
                .With(p => p.QuoteGuid)
                .With(p => p.QuotedGuid, authorGuid).Create();

            _builder = new HttpRequestBuilder()
                .InitializeRequest()
                .WithMethod(HttpMethod.Post)
                .WithUri("http://localhost:64892/api/quotes/author")
                .AddBearerToken(token.AccessToken);
        }

        public CreateAuthorQuoteHttpRequest WithQuoteGuid(Guid quoteGuid)
        {
            _writeModel.QuoteGuid = quoteGuid;

            return this;
        }

        public HttpRequestMessage ToHttpRequest()
        {
            return _builder.WithStringContent(_writeModel).GetRequest();
        }
    }
}