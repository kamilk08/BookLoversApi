using BookLovers.Base.Infrastructure.Events.DomainEvents;
using BookLovers.Base.Infrastructure.Events.IntegrationEvents;
using BookLovers.Base.Infrastructure.ModuleCommunication;
using BookLovers.Bookcases.Application.Commands.Bookcases;
using Ninject.Extensions.Conventions;
using Ninject.Modules;

namespace BookLovers.Bookcases.Infrastructure.Root.Events
{
    public class EventsModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IInMemoryEventBus>().To<InMemoryEventBus>().InSingletonScope();

            Bind<IProjectionDispatcher>().To<ProjectionDispatcher>();

            this.Bind(x =>
                x.FromAssemblyContaining(typeof(AddToBookcaseCommand))
                    .IncludingNonPublicTypes().SelectAllClasses()
                    .InheritedFrom(typeof(IDomainEventHandler<>))
                    .BindAllInterfaces());

            this.Bind(x =>
                x.FromThisAssembly().IncludingNonPublicTypes()
                    .SelectAllClasses()
                    .InheritedFrom(typeof(IProjectionHandler<>))
                    .BindAllInterfaces());

            Bind<IProjectionDispatcher>()
                .To<ProjectionDispatcherLoggingDecorator>()
                .WhenInjectedInto(typeof(UnitOfWork));

            Bind<IIntegrationEventDispatcher>().To<IntegrationEventDispatcher>();

            this.Bind(x =>
                x.FromAssemblyContaining(typeof(AddToBookcaseCommand))
                    .IncludingNonPublicTypes().SelectAllClasses()
                    .InheritedFrom(typeof(IIntegrationEventHandler<>))
                    .BindAllInterfaces());

            Bind<IDomainEventDispatcher>()
                .To<DomainEventDispatcher>();

            Bind<IDomainEventDispatcher>()
                .To<DomainEventDispatcherLoggingDecorator>()
                .WhenInjectedInto(typeof(UnitOfWork));

            Bind<IDomainEventDispatcher>().To<DomainEventDispatcher>()
                .WhenInjectedInto(typeof(DomainEventDispatcherLoggingDecorator));
        }
    }
}