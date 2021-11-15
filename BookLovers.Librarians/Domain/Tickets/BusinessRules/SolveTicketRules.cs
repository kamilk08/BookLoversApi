using System.Collections.Generic;
using BookLovers.Base.Domain.Rules;

namespace BookLovers.Librarians.Domain.Tickets.BusinessRules
{
    internal sealed class SolveTicketRules : BaseBusinessRule, IBusinessRule
    {
        protected override List<IBusinessRule> FollowingRules { get; } =
            new List<IBusinessRule>();

        public SolveTicketRules(Ticket ticket)
        {
            this.FollowingRules.Add(new AggregateMustExist(ticket.Guid));
            this.FollowingRules.Add(new AggregateMustBeActive(ticket.Status));
            this.FollowingRules.Add(new TicketCannotBeSolvedTwiceOrMore(ticket));
        }

        public bool IsFulfilled() => !this.AreFollowingRulesBroken();

        public string BrokenRuleMessage => this.Message;
    }
}