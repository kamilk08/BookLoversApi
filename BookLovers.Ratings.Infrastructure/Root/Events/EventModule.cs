using BookLovers.Base.Infrastructure.Events.DomainEvents;
using BookLovers.Base.Infrastructure.Events.IntegrationEvents;
using BookLovers.Base.Infrastructure.ModuleCommunication;
using BookLovers.Ratings.Application.Commands;
using Ninject.Extensions.Conventions;
using Ninject.Modules;

namespace BookLovers.Ratings.Infrastructure.Root.Events
{
    internal class EventModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IInMemoryEventBus>().To<InMemoryEventBus>().InSingletonScope();

            this.Bind(x =>
                x.FromAssemblyContaining(typeof(AddRatingCommand))
                    .IncludingNonPublicTypes().SelectAllClasses()
                    .InheritedFrom(typeof(IIntegrationEventHandler<>))
                    .BindAllInterfaces());

            Bind<IIntegrationEventDispatcher>().To<IntegrationEventDispatcher>();

            this.Bind(x =>
                x.FromAssemblyContaining(typeof(AddRatingCommand))
                    .IncludingNonPublicTypes().SelectAllClasses()
                    .InheritedFrom(typeof(IDomainEventHandler<>))
                    .BindAllInterfaces());

            Bind<IDomainEventDispatcher>().To<DomainEventDispatcher>();

            Bind<IDomainEventDispatcher>().To<DomainEventDispatcherLoggingDecorator>()
                .WhenInjectedInto(typeof(UnitOfWork));

            Bind<IDomainEventDispatcher>().To<DomainEventDispatcher>()
                .WhenInjectedInto(typeof(DomainEventDispatcherLoggingDecorator));
        }
    }
}