using System.Threading.Tasks;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Base.Infrastructure.Events.DomainEvents;
using BookLovers.Readers.Application.Commands.NotificationWalls;
using BookLovers.Readers.Application.Commands.Profile;
using BookLovers.Readers.Events.Readers;

namespace BookLovers.Readers.Application.Events.Readers
{
    internal class ReaderSuspendedEventHandler :
        IDomainEventHandler<ReaderSuspended>,
        IDomainEventHandler
    {
        private readonly IInternalCommandDispatcher _commandDispatcher;

        public ReaderSuspendedEventHandler(IInternalCommandDispatcher commandDispatcher)
        {
            _commandDispatcher = commandDispatcher;
        }

        public async Task HandleAsync(ReaderSuspended @event)
        {
            await _commandDispatcher.SendInternalCommandAsync(new ArchiveProfileInternalCommand(@event.ProfileGuid));

            await _commandDispatcher.SendInternalCommandAsync(
                new ArchiveNotificationWallInternalCommand(@event.NotificationWallGuid));
        }
    }
}