using System;
using System.Collections.Generic;
using BookLovers.Base.Domain.Rules;
using BookLovers.Base.Infrastructure.Services;
using BookLovers.Librarians.Domain.TicketOwners;
using BookLovers.Librarians.Domain.Tickets.BusinessRules;
using BookLovers.Librarians.Domain.Tickets.Services.Factories;
using BookLovers.Librarians.Events.Tickets;

namespace BookLovers.Librarians.Domain.Tickets.Services
{
    public class TicketFactory
    {
        private readonly List<Func<Ticket, IBusinessRule>> _businessRules =
            new List<Func<Ticket, IBusinessRule>>();

        private ITicketConcernProvider _concernProvider;
        private ITicketOwnerRepository _ticketOwnerRepository;
        private IHttpContextAccessor _httpContextAccessor;
        private ITicketConcernChecker _checker;
        private TicketFactoryData _factoryData;

        internal TicketFactory(TicketFactoryData factoryData)
        {
            this._factoryData = factoryData;
        }

        public TicketFactory()
            : this(TicketFactoryData.Initialize())
        {
            this._businessRules.Add(ticket => new AggregateMustBeActive(ticket.Status));
            this._businessRules.Add(ticket => new TicketMustBeInProgress(ticket));
            this._businessRules.Add(ticket => new TicketConcernMustBeValid(this._checker, ticket));
            this._businessRules.Add(ticket => new TicketMustHaveHisOwner(ticket));
            this._businessRules.Add(ticket => new TicketCannotBeSolvedTwiceOrMore(ticket));
            this._businessRules.Add(ticket => new CreatedTicketCannotHaveDecision(ticket));
        }

        public Ticket Create()
        {
            var ticketDetails = new TicketDetails(
                this._factoryData.TicketContentData.Title,
                this._factoryData.TicketDetailsData.Description,
                this._factoryData.TicketDetailsData.CreatedAt);

            var ticketContent = new TicketContent(
                this._factoryData.TicketObjectGuid,
                this._factoryData.TicketContentData.TicketData,
                this._concernProvider.GetTicketConcern(this._factoryData.TicketDetailsData.TicketConcern));

            var ownerByReaderGuid = this._ticketOwnerRepository.GetOwnerByReaderGuid(this._httpContextAccessor.UserGuid);

            if (ownerByReaderGuid == null)
                throw new BusinessRuleNotMetException("User does not exist.");

            var issuedBy = new IssuedBy(ownerByReaderGuid.ReaderId, ownerByReaderGuid.ReaderGuid);
            var ticket = new Ticket(this._factoryData.TicketGuid, issuedBy, ticketContent, ticketDetails);

            foreach (var businessRule in this._businessRules)
            {
                if (!businessRule(ticket).IsFulfilled())
                    throw new BusinessRuleNotMetException(businessRule(ticket).BrokenRuleMessage);
            }

            var ticketCreated = TicketCreated.Initialize()
                .WithAggregate(ticket.Guid)
                .WithTicketOwner(issuedBy.TicketOwnerGuid)
                .WithTicket(ticketDetails.Title, ticketContent.Content)
                .WithDate(ticketDetails.Date);

            ticket.AddEvent(ticketCreated);

            return ticket;
        }

        internal TicketFactory Set(ITicketConcernProvider concernProvider)
        {
            this._concernProvider = concernProvider;

            return this;
        }

        internal TicketFactory Set(ITicketOwnerRepository ownerRepository)
        {
            this._ticketOwnerRepository = ownerRepository;

            return this;
        }

        internal TicketFactory Set(IHttpContextAccessor httpContextAccessor)
        {
            this._httpContextAccessor = httpContextAccessor;

            return this;
        }

        internal TicketFactory Set(TicketFactoryData factoryData)
        {
            this._factoryData = factoryData;

            return this;
        }

        internal TicketFactory Set(ITicketConcernChecker checker)
        {
            this._checker = checker;

            return this;
        }
    }
}