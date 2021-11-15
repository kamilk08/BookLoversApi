using System;
using System.Net.Http;
using AutoFixture;
using BookLovers.Base.Infrastructure;
using BookLovers.Publication.Application.WriteModels.PublisherCycles;
using FluentHttpRequestBuilderLibrary;
using SimpleE2ETesterLibrary.Interfaces;

namespace BaseTests.EndToEndHelpers.HttpRequests.Publications
{
    public class AddToPublisherCycleHttpRequest : ISimpleHttpRequest
    {
        private readonly HttpRequestBuilder _builder;
        private readonly AddCycleBookWriteModel _writeModel;

        public AddToPublisherCycleHttpRequest(
            Guid bookGuid,
            Guid publisherCycleGuid,
            JsonWebToken token)
        {
            _writeModel = new Fixture().Build<AddCycleBookWriteModel>()
                .With(p => p.BookGuid, bookGuid)
                .With(p => p.CycleGuid, publisherCycleGuid).Create();

            _builder = new HttpRequestBuilder()
                .InitializeRequest()
                .WithMethod(HttpMethod.Put)
                .WithUri("http://localhost:64892/api/cycles/book")
                .AddBearerToken(token.AccessToken);
        }

        public HttpRequestMessage ToHttpRequest()
        {
            return _builder.WithStringContent(_writeModel).GetRequest();
        }
    }
}