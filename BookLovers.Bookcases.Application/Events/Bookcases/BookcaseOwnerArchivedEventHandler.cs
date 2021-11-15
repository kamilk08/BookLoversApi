using System.Threading.Tasks;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Base.Infrastructure.Events.DomainEvents;
using BookLovers.Bookcases.Application.Commands.Bookcases;
using BookLovers.Bookcases.Events.Readers;

namespace BookLovers.Bookcases.Application.Events.Bookcases
{
    internal class BookcaseOwnerArchivedEventHandler :
        IDomainEventHandler<BookcaseOwnerArchived>,
        IDomainEventHandler
    {
        private readonly IInternalCommandDispatcher _commandDispatcher;

        public BookcaseOwnerArchivedEventHandler(IInternalCommandDispatcher commandDispatcher)
        {
            _commandDispatcher = commandDispatcher;
        }

        public Task HandleAsync(BookcaseOwnerArchived @event)
        {
            var command = new ArchiveBookcaseInternalCommand(@event.BookcaseGuid);

            return _commandDispatcher.SendInternalCommandAsync(command);
        }
    }
}