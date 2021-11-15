using System.Threading.Tasks;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Base.Infrastructure.Events.DomainEvents;
using BookLovers.Publication.Application.Commands.Publishers;
using BookLovers.Publication.Events.PublisherCycles;

namespace BookLovers.Publication.Application.Events.Publishers
{
    internal class CycleCreatedEventHandler :
        IDomainEventHandler<PublisherCycleCreated>,
        IDomainEventHandler
    {
        private readonly IInternalCommandDispatcher _commandDispatcher;

        public CycleCreatedEventHandler(IInternalCommandDispatcher commandDispatcher)
        {
            this._commandDispatcher = commandDispatcher;
        }

        public Task HandleAsync(PublisherCycleCreated @event)
        {
            return this._commandDispatcher.SendInternalCommandAsync(
                new AddPublisherCycleInternalCommand(@event.PublisherGuid, @event.AggregateGuid));
        }
    }
}