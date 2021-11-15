using System;
using System.Net.Http;
using AutoFixture;
using BookLovers.Base.Infrastructure;
using BookLovers.Librarians.Application.WriteModels;
using BookLovers.Librarians.Domain.Librarians;
using BookLovers.Librarians.Domain.Tickets;
using FluentHttpRequestBuilderLibrary;
using SimpleE2ETesterLibrary.Interfaces;

namespace BaseTests.EndToEndHelpers.HttpRequests.Librarians
{
    public class ResolveTicketHttpRequest : ISimpleHttpRequest
    {
        private readonly HttpRequestBuilder _builder;
        private ResolveTicketWriteModel _writeModel;

        public ResolveTicketHttpRequest(
            Guid ticketGuid,
            Guid librarianGuid,
            Decision decision,
            TicketConcern ticketConcern,
            JsonWebToken token)
        {
            var fixture = new Fixture();

            _builder = new HttpRequestBuilder()
                .InitializeRequest()
                .WithMethod(HttpMethod.Put)
                .WithUri("http://localhost:64892/api/tickets")
                .AddBearerToken(token.AccessToken);

            _writeModel = fixture.Build<ResolveTicketWriteModel>()
                .With(p => p.Date)
                .With(p => p.DecisionJustification)
                .With(p => p.DecisionType, decision.Value)
                .With(p => p.TicketConcern, ticketConcern.Value)
                .With(p => p.TicketGuid, ticketGuid)
                .With(p => p.LibrarianGuid, librarianGuid)
                .Create();
        }

        public HttpRequestMessage ToHttpRequest()
        {
            return _builder.WithStringContent(_writeModel).GetRequest();
        }
    }
}