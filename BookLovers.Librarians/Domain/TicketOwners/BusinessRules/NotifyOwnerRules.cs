using System;
using System.Collections.Generic;
using BookLovers.Base.Domain.Rules;

namespace BookLovers.Librarians.Domain.TicketOwners.BusinessRules
{
    internal sealed class NotifyOwnerRules : BaseBusinessRule, IBusinessRule
    {
        protected override List<IBusinessRule> FollowingRules { get; } =
            new List<IBusinessRule>();

        public NotifyOwnerRules(TicketOwner ticketOwner, Guid issuedByGuid, Guid ticketGuid)
        {
            FollowingRules.Add(new AggregateMustExist(ticketOwner.Guid));
            FollowingRules.Add(new AggregateMustBeActive(ticketOwner.Status));
            FollowingRules.Add(new OwnerMustHaveSelectedTicket(ticketOwner, ticketGuid));
            FollowingRules.Add(new TicketMustBeIssuedByTicketOwner(ticketOwner, issuedByGuid));
        }

        public bool IsFulfilled() => !AreFollowingRulesBroken();

        public string BrokenRuleMessage => Message;
    }
}