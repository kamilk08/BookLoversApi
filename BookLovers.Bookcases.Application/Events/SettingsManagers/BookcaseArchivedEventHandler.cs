using System.Threading.Tasks;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Base.Infrastructure.Events.DomainEvents;
using BookLovers.Bookcases.Application.Commands;
using BookLovers.Bookcases.Events.Bookcases;

namespace BookLovers.Bookcases.Application.Events.SettingsManagers
{
    internal class BookcaseArchivedEventHandler :
        IDomainEventHandler<BookcaseArchived>,
        IDomainEventHandler
    {
        private readonly IInternalCommandDispatcher _commandDispatcher;

        public BookcaseArchivedEventHandler(IInternalCommandDispatcher commandDispatcher)
        {
            _commandDispatcher = commandDispatcher;
        }

        public Task HandleAsync(BookcaseArchived @event)
        {
            var command = new ArchiveSettingsManagerInternalCommand(@event.SettingsManagerGuid);

            return _commandDispatcher.SendInternalCommandAsync(command);
        }
    }
}