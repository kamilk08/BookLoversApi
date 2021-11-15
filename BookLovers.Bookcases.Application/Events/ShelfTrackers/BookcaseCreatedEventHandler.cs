using System.Threading.Tasks;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Base.Infrastructure.Events.DomainEvents;
using BookLovers.Bookcases.Application.Commands.ShelfTrackers;
using BookLovers.Bookcases.Events.Bookcases;

namespace BookLovers.Bookcases.Application.Events.ShelfTrackers
{
    internal class BookcaseCreatedEventHandler :
        IDomainEventHandler<BookcaseCreated>
    {
        private readonly IInternalCommandDispatcher _commandDispatcher;

        public BookcaseCreatedEventHandler(IInternalCommandDispatcher commandDispatcher)
        {
            _commandDispatcher = commandDispatcher;
        }

        public Task HandleAsync(BookcaseCreated @event)
        {
            var command = new CreateShelfRecordTrackerInternalCommand(@event.AggregateGuid, @event.ShelfTrackerGuid);

            return _commandDispatcher.SendInternalCommandAsync(command);
        }
    }
}