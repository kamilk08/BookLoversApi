using BookLovers.Auth.Application.Commands.Users;
using BookLovers.Base.Infrastructure.Events.DomainEvents;
using BookLovers.Base.Infrastructure.Events.IntegrationEvents;
using BookLovers.Base.Infrastructure.ModuleCommunication;
using Ninject.Extensions.Conventions;
using Ninject.Modules;

namespace BookLovers.Auth.Infrastructure.Root.Events
{
    internal class EventsModule : NinjectModule
    {
        public override void Load()
        {
            this.Bind(x =>
                x.FromAssemblyContaining(typeof(SignUpCommand))
                    .IncludingNonPublicTypes().SelectAllClasses()
                    .InheritedFrom(typeof(IIntegrationEventHandler<>))
                    .BindAllInterfaces());

            Bind<IIntegrationEventDispatcher>().To<IntegrationEventDispatcher>();

            this.Bind(x =>
                x.FromAssemblyContaining(typeof(SignUpCommand))
                    .IncludingNonPublicTypes().SelectAllClasses()
                    .InheritedFrom(typeof(IDomainEventHandler<>))
                    .BindAllInterfaces());

            Bind<IInMemoryEventBus>().To<InMemoryEventBus>().InSingletonScope();

            Bind<IDomainEventDispatcher>().To<DomainEventDispatcher>();

            Bind<IDomainEventDispatcher>()
                .To<DomainEventDispatcherLoggingDecorator>()
                .WhenInjectedInto(typeof(UnitOfWork));

            Bind<IDomainEventDispatcher>()
                .To<DomainEventDispatcher>()
                .WhenInjectedInto(typeof(DomainEventDispatcherLoggingDecorator));
        }
    }
}