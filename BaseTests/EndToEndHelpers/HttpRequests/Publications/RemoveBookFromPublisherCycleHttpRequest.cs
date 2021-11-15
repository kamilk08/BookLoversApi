using System;
using System.Net.Http;
using AutoFixture;
using BookLovers.Base.Infrastructure;
using BookLovers.Publication.Application.WriteModels.PublisherCycles;
using FluentHttpRequestBuilderLibrary;
using SimpleE2ETesterLibrary.Interfaces;

namespace BaseTests.EndToEndHelpers.HttpRequests.Publications
{
    public class RemoveBookFromPublisherCycleHttpRequest : ISimpleHttpRequest
    {
        private readonly HttpRequestBuilder _builder;
        private RemoveCycleBookWriteModel _writeModel;

        public RemoveBookFromPublisherCycleHttpRequest(
            Guid bookGuid,
            Guid publisherCycleGuid,
            JsonWebToken token)
        {
            _builder = new HttpRequestBuilder();
            _writeModel = new Fixture()
                .Build<RemoveCycleBookWriteModel>()
                .With(p => p.BookGuid, bookGuid)
                .With(p => p.CycleGuid, publisherCycleGuid)
                .Create();

            _builder.InitializeRequest()
                .WithMethod(HttpMethod.Delete)
                .WithUri("http://localhost:64892/api/cycles/book")
                .AddBearerToken(token.AccessToken);
        }

        public HttpRequestMessage ToHttpRequest()
        {
            return _builder.WithStringContent(_writeModel).GetRequest();
        }
    }
}