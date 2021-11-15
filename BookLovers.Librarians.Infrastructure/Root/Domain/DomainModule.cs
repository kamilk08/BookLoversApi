using BookLovers.Base.Domain.Services;
using BookLovers.Librarians.Domain.Librarians;
using BookLovers.Librarians.Domain.Librarians.DecisionNotifiers;
using BookLovers.Librarians.Domain.PromotionWaiters;
using BookLovers.Librarians.Domain.ReviewReportRegisters;
using BookLovers.Librarians.Domain.Tickets.Services;
using BookLovers.Librarians.Domain.Tickets.Services.Factories;
using BookLovers.Librarians.Domain.Tickets.TicketReasons;
using BookLovers.Librarians.Infrastructure.Persistence.Providers;
using BookLovers.Librarians.Infrastructure.Persistence.Repositories;
using Ninject.Extensions.Conventions;
using Ninject.Modules;
using Ninject.Web.Common;
using System.Collections.Generic;

namespace BookLovers.Librarians.Infrastructure.Root.Domain
{
    internal class DomainModule : NinjectModule
    {
        public override void Load()
        {
            this.Bind(x =>
                x.FromAssemblyContaining(typeof(Librarian))
                    .IncludingNonPublicTypes().SelectAllClasses()
                    .InheritedFrom(typeof(IDecisionNotifier))
                    .BindAllInterfaces());

            this.Bind(x =>
                x.FromAssemblyContaining(typeof(Librarian))
                    .IncludingNonPublicTypes().SelectAllClasses()
                    .InheritedFrom(typeof(IDecisionGiver))
                    .BindAllInterfaces());

            Bind<ITicketConcernProvider>().To<TicketConcernProvider>();

            Bind<IDecisionProvider>().To<DecisionProvider>();

            Bind<IDictionary<ITicketSummary, IDecisionNotifier>>()
                .ToProvider<NotifiersProvider>();

            Bind<IDictionary<Decision, IDecisionGiver>>()
                .ToProvider<DecisionGiverProvider>();

            Bind<TicketDecisionNotifier>().ToSelf();

            Bind<TicketFactory>().ToSelf();

            Bind<IReportReasonProvider>().To<ReportReasonProvider>();

            this.Bind(x =>
                x.FromAssemblyContaining(typeof(Librarian))
                    .IncludingNonPublicTypes()
                    .SelectAllClasses()
                    .InheritedFrom(typeof(IAggregateManager<>))
                    .BindAllInterfaces());

            Bind<IPromotionAvailabilityProvider>()
                .To<PromotionAvailabilityProvider>().InRequestScope();

            Bind<TicketFactorySetup>().ToSelf();

            Bind<ITicketConcernChecker>().To<TicketConcernProvider>();

            Bind<IDecisionChecker>().To<DecisionProvider>();
        }
    }
}