using System.Threading.Tasks;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Base.Infrastructure.Events.DomainEvents;
using BookLovers.Bookcases.Application.Commands;
using BookLovers.Bookcases.Events.Bookcases;

namespace BookLovers.Bookcases.Application.Events.SettingsManagers
{
    internal class BookcaseCreatedEventHandler :
        IDomainEventHandler<BookcaseCreated>,
        IDomainEventHandler
    {
        private readonly IInternalCommandDispatcher _commandDispatcher;

        public BookcaseCreatedEventHandler(IInternalCommandDispatcher commandDispatcher)
        {
            _commandDispatcher = commandDispatcher;
        }

        public Task HandleAsync(BookcaseCreated @event)
        {
            var command = new CreateSettingsManagerInternalCommand(@event.AggregateGuid, @event.SettingsManagerGuid);

            return _commandDispatcher.SendInternalCommandAsync(command);
        }
    }
}