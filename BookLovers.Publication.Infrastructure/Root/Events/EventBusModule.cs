using BookLovers.Base.Infrastructure.Events.DomainEvents;
using BookLovers.Base.Infrastructure.Events.InfrastructureEvents;
using BookLovers.Base.Infrastructure.Events.IntegrationEvents;
using BookLovers.Base.Infrastructure.ModuleCommunication;
using BookLovers.Publication.Application.Commands.Books;
using Ninject.Extensions.Conventions;
using Ninject.Modules;

namespace BookLovers.Publication.Infrastructure.Root.Events
{
    internal class EventBusModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IInMemoryEventBus>().To<InMemoryEventBus>().InSingletonScope();

            Bind<IProjectionDispatcher>().To<ProjectionDispatcher>();

            Bind<IInfrastructureEventDispatcher>()
                .To<InfrastructureEventDispatcher>();

            this.Bind(x =>
                x.FromAssemblyContaining(typeof(AddBookCommand))
                    .IncludingNonPublicTypes().SelectAllClasses()
                    .InheritedFrom(typeof(IDomainEventHandler<>))
                    .BindAllInterfaces());

            this.Bind(x =>
                x.FromThisAssembly().IncludingNonPublicTypes()
                    .SelectAllClasses()
                    .InheritedFrom(typeof(IProjectionHandler<>))
                    .BindAllInterfaces());

            this.Bind(x =>
                x.FromAssemblyContaining(typeof(AddBookCommand))
                    .IncludingNonPublicTypes().SelectAllClasses()
                    .InheritedFrom(typeof(IIntegrationEventHandler<>))
                    .BindAllInterfaces());

            Bind<IIntegrationEventDispatcher>().To<IntegrationEventDispatcher>();

            this.Bind(x =>
                x.FromThisAssembly()
                    .IncludingNonPublicTypes()
                    .SelectAllClasses()
                    .InheritedFrom(typeof(IInfrastructureEventHandler<>))
                    .BindAllInterfaces());

            Bind<IDomainEventDispatcher>().To<DomainEventDispatcher>();

            Bind<IDomainEventDispatcher>()
                .To<DomainEventDispatcherLoggingDecorator>()
                .WhenInjectedInto(typeof(UnitOfWork));

            Bind<IDomainEventDispatcher>().To<DomainEventDispatcher>()
                .WhenInjectedInto(typeof(DomainEventDispatcherLoggingDecorator));
        }
    }
}