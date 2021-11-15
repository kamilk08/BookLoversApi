using BookLovers.Base.Infrastructure.Ioc;
using BookLovers.Librarians.Domain.Librarians;
using BookLovers.Librarians.Domain.Tickets.Services;
using Ninject;
using Ninject.Activation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BookLovers.Librarians.Infrastructure.Root.Domain
{
    public class DecisionGiverProvider : BaseProvider<IDictionary<Decision, IDecisionGiver>>
    {
        public override Type Type => typeof(IDictionary<Decision, IDecisionGiver>);

        protected override IDictionary<Decision, IDecisionGiver> CreateInstance(
            IContext context)
        {
            return context.Kernel.GetAll<IDecisionGiver>()
                .ToDictionary(k => k.Decision, v => v);
        }
    }
}