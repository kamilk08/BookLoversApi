using System.Threading.Tasks;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Base.Infrastructure.Events.DomainEvents;
using BookLovers.Readers.Application.Commands.Timelines;
using BookLovers.Readers.Events.Readers;

namespace BookLovers.Readers.Application.Events.Readers.Books
{
    internal class ReaderAddedBookEventHandler :
        IDomainEventHandler<ReaderAddedBook>,
        IDomainEventHandler
    {
        private readonly IInternalCommandDispatcher _commandDispatcher;

        public ReaderAddedBookEventHandler(IInternalCommandDispatcher commandDispatcher)
        {
            _commandDispatcher = commandDispatcher;
        }

        public Task HandleAsync(ReaderAddedBook @event)
        {
            return _commandDispatcher.SendInternalCommandAsync(
                new AddBookActivityInternalCommand(@event.BookGuid, @event.AggregateGuid, @event.AddedAt));
        }
    }
}