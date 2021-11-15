using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookLovers.Librarians.Domain.Tickets;
using BookLovers.Librarians.Domain.Tickets.Services;
using BookLovers.Librarians.Domain.Tickets.TicketReasons;

namespace BookLovers.Librarians.Domain.Librarians.DecisionNotifiers
{
    public class TicketDecisionNotifier
    {
        private readonly IDictionary<ITicketSummary, IDecisionNotifier> _notifiers;
        private readonly IDictionary<Decision, IDecisionGiver> _decisionGivers;

        public TicketDecisionNotifier(
            IDictionary<ITicketSummary, IDecisionNotifier> notifiers,
            IDictionary<Decision, IDecisionGiver> decisionGivers)
        {
            this._notifiers = notifiers;
            this._decisionGivers = decisionGivers;
        }

        public async Task Notify(Ticket ticket, string justification)
        {
            var notifier = this.GetNotifier(ticket.TicketContent.TicketConcern, ticket.Decision);

            if (notifier == null)
                throw new InvalidOperationException("Cannot notify owner of a ticket.");

            await notifier.Notify(ticket, justification);
        }

        private IDecisionNotifier GetNotifier(TicketConcern ticketConcern, Decision decision)
        {
            var ticketDecision = this._decisionGivers
                .Single(p => p.Key.Value == decision.Value).Value
                .GiveDecision();

            return this._notifiers.SingleOrDefault(
                p =>
                    p.Key.TicketConcern.Value == ticketConcern.Value &&
                    p.Key.DecisionMade.Value == ticketDecision.Value).Value;
        }
    }
}