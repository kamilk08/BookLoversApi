using System;
using System.Net.Http;
using AutoFixture;
using BookLovers.Base.Infrastructure;
using BookLovers.Librarians.Application.WriteModels;
using BookLovers.Librarians.Domain.Tickets;
using FluentHttpRequestBuilderLibrary;
using SimpleE2ETesterLibrary.Interfaces;

namespace BaseTests.EndToEndHelpers.HttpRequests.Librarians
{
    public class CreateTicketHttpRequest : ISimpleHttpRequest
    {
        private readonly HttpRequestBuilder _builder;
        private readonly CreateTicketWriteModel _writeModel;

        public Guid TicketGuid => _writeModel.TicketGuid;

        public CreateTicketHttpRequest(JsonWebToken token)
        {
            _writeModel = new Fixture().Build<CreateTicketWriteModel>()
                .With(p => p.Description)
                .With(p => p.Title).With(p => p.CreatedAt)
                .Without(p => p.TicketConcern)
                .Without(p => p.TicketData).Create();

            _builder = new HttpRequestBuilder()
                .InitializeRequest()
                .WithMethod(HttpMethod.Post)
                .WithUri("http://localhost:64892/api/tickets")
                .AddBearerToken(token.AccessToken);
        }

        public CreateTicketHttpRequest WithTicketConcern(TicketConcern concern)
        {
            _writeModel.TicketConcern = concern.Value;

            return this;
        }

        public CreateTicketHttpRequest WithTicketData(
            Guid ticketObjectGuid,
            string ticketData)
        {
            _writeModel.TicketData = ticketData;
            _writeModel.TicketObjectGuid = ticketObjectGuid;

            return this;
        }

        public CreateTicketHttpRequest WithTicketGuid(Guid ticketGuid)
        {
            _writeModel.TicketGuid = ticketGuid;

            return this;
        }

        public HttpRequestMessage ToHttpRequest()
        {
            return _builder.WithStringContent(_writeModel).GetRequest();
        }
    }
}