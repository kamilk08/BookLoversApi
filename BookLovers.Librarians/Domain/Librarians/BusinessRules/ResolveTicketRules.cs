using System;
using System.Collections.Generic;
using BookLovers.Base.Domain.Aggregates;
using BookLovers.Base.Domain.Rules;
using BookLovers.Librarians.Domain.Tickets;

namespace BookLovers.Librarians.Domain.Librarians.BusinessRules
{
    internal sealed class ResolveTicketRules : BaseBusinessRule, IBusinessRule
    {
        protected sealed override List<IBusinessRule> FollowingRules { get; }
            = new List<IBusinessRule>();

        public ResolveTicketRules(Librarian librarian, Ticket ticket, IDecisionChecker checker)
        {
            this.FollowingRules.Add(new AggregateMustExist(ticket?.Guid ?? Guid.Empty));
            this.FollowingRules.Add(new AggregateMustExist(librarian.Guid));

            this.FollowingRules.Add(new AggregateMustBeActive(ticket?.Status ?? AggregateStatus.Archived.Value));
            this.FollowingRules.Add(new AggregateMustBeActive(librarian.Status));

            this.FollowingRules.Add(new TicketDecisionMustBeValid(ticket, checker));
            this.FollowingRules.Add(new TicketCanBeResolvedOnlyOnce(librarian, ticket));
        }

        public bool IsFulfilled() => !this.AreFollowingRulesBroken();

        public string BrokenRuleMessage => this.Message;
    }
}