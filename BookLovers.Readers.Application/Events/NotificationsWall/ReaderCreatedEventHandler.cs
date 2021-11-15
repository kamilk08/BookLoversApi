using System.Threading.Tasks;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Base.Infrastructure.Events.DomainEvents;
using BookLovers.Readers.Application.Commands.NotificationWalls;
using BookLovers.Readers.Events.Readers;

namespace BookLovers.Readers.Application.Events.NotificationsWall
{
    internal class ReaderCreatedEventHandler : IDomainEventHandler<ReaderCreated>, IDomainEventHandler
    {
        private readonly IInternalCommandDispatcher _commandDispatcher;

        public ReaderCreatedEventHandler(IInternalCommandDispatcher commandDispatcher)
        {
            _commandDispatcher = commandDispatcher;
        }

        public Task HandleAsync(ReaderCreated @event)
        {
            return _commandDispatcher.SendInternalCommandAsync(
                new CreateNotificationWallInternalCommand(@event.NotificationWallGuid, @event.AggregateGuid));
        }
    }
}