using BookLovers.Base.Infrastructure.Events.DomainEvents;
using BookLovers.Base.Infrastructure.Events.InfrastructureEvents;
using BookLovers.Base.Infrastructure.Events.IntegrationEvents;
using BookLovers.Base.Infrastructure.ModuleCommunication;
using BookLovers.Readers.Application.Commands.Reviews;
using Ninject.Extensions.Conventions;
using Ninject.Modules;

namespace BookLovers.Readers.Infrastructure.Root.Events
{
    internal class EventsModule : NinjectModule
    {
        public override void Load()
        {
            this.Bind<IInMemoryEventBus>().To<InMemoryEventBus>().InSingletonScope();

            this.Bind<IProjectionDispatcher>().To<ProjectionDispatcher>();

            this.Bind<IInfrastructureEventDispatcher>().To<InfrastructureEventDispatcher>();

            this.Bind(x =>
                x.FromAssemblyContaining(typeof(AddReviewCommand))
                    .IncludingNonPublicTypes().SelectAllClasses()
                    .InheritedFrom(typeof(IDomainEventHandler<>))
                    .BindAllInterfaces());

            this.Bind(x =>
                x.FromThisAssembly()
                    .IncludingNonPublicTypes()
                    .SelectAllClasses()
                    .InheritedFrom(typeof(IProjectionHandler<>))
                    .BindAllInterfaces());

            this.Bind(x =>
                x.FromAssemblyContaining(typeof(AddReviewCommand))
                    .IncludingNonPublicTypes().SelectAllClasses()
                    .InheritedFrom(typeof(IIntegrationEventHandler<>))
                    .BindAllInterfaces());

            this.Bind<IIntegrationEventDispatcher>().To<IntegrationEventDispatcher>();

            this.Bind(x =>
                x.FromThisAssembly().IncludingNonPublicTypes()
                    .SelectAllClasses()
                    .InheritedFrom(typeof(IInfrastructureEventHandler<>))
                    .BindAllInterfaces());

            this.Bind<IDomainEventDispatcher>().To<DomainEventDispatcher>();
        }
    }
}