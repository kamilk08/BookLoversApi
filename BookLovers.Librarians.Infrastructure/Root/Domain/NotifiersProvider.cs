using BookLovers.Base.Infrastructure.Ioc;
using BookLovers.Librarians.Domain.Librarians.DecisionNotifiers;
using BookLovers.Librarians.Domain.Tickets.TicketReasons;
using Ninject;
using Ninject.Activation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BookLovers.Librarians.Infrastructure.Root.Domain
{
    internal class NotifiersProvider : BaseProvider<Dictionary<ITicketSummary, IDecisionNotifier>>
    {
        public override Type Type => typeof(Dictionary<ITicketSummary, IDecisionNotifier>);

        protected override Dictionary<ITicketSummary, IDecisionNotifier> CreateInstance(
            IContext context)
        {
            return context.Kernel.GetAll<IDecisionNotifier>()
                .ToDictionary(k => k.TicketSummary, v => v);
        }
    }
}