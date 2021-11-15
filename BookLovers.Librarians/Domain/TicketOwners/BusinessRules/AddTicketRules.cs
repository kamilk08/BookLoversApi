using System.Collections.Generic;
using BookLovers.Base.Domain.Rules;
using BookLovers.Librarians.Domain.Tickets;

namespace BookLovers.Librarians.Domain.TicketOwners.BusinessRules
{
    internal sealed class AddTicketRules : BaseBusinessRule, IBusinessRule
    {
        protected override List<IBusinessRule> FollowingRules { get; } = new List<IBusinessRule>();

        public AddTicketRules(TicketOwner ticketOwner, Ticket ticket)
        {
            FollowingRules.Add(new AggregateMustExist(ticket.Guid));
            FollowingRules.Add(new AggregateMustExist(ticketOwner.Guid));
            FollowingRules.Add(new AggregateMustBeActive(ticketOwner.Status));
            FollowingRules.Add(new TicketOwnerCannotHaveDuplicatedTickets(ticketOwner, ticket));
            FollowingRules.Add(new TicketMustBeIssuedByTicketOwner(ticketOwner, ticket.IssuedBy.TicketOwnerGuid));
        }

        public bool IsFulfilled() => !AreFollowingRulesBroken();

        public string BrokenRuleMessage => Message;
    }
}