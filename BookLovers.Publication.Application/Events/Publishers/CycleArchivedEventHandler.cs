using System.Threading.Tasks;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Base.Infrastructure.Events.DomainEvents;
using BookLovers.Publication.Application.Commands.Publishers;
using BookLovers.Publication.Events.PublisherCycles;

namespace BookLovers.Publication.Application.Events.Publishers
{
    internal class CycleArchivedEventHandler :
        IDomainEventHandler<PublisherCycleArchived>,
        IDomainEventHandler
    {
        private readonly IInternalCommandDispatcher _commandDispatcher;

        public CycleArchivedEventHandler(IInternalCommandDispatcher commandDispatcher)
        {
            this._commandDispatcher = commandDispatcher;
        }

        public Task HandleAsync(PublisherCycleArchived @event)
        {
            return this._commandDispatcher.SendInternalCommandAsync(
                new RemovePublisherCycleInternalCommand(@event.PublisherGuid, @event.AggregateGuid));
        }
    }
}