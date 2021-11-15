using System.Threading.Tasks;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Base.Infrastructure.Events.DomainEvents;
using BookLovers.Readers.Application.Commands.NotificationWalls;
using BookLovers.Readers.Events.Readers;

namespace BookLovers.Readers.Application.Events.NotificationsWall.Followers
{
    internal class ReaderUnFollowedEventHandler :
        IDomainEventHandler<ReaderUnFollowed>,
        IDomainEventHandler
    {
        private readonly IInternalCommandDispatcher _commandDispatcher;

        public ReaderUnFollowedEventHandler(IInternalCommandDispatcher commandDispatcher)
        {
            _commandDispatcher = commandDispatcher;
        }

        public Task HandleAsync(ReaderUnFollowed @event)
        {
            return _commandDispatcher.SendInternalCommandAsync(
                new AddRemoveFollowerNotificationInternalCommand(@event.NotificationWallGuid, @event.UnFollowedByGuid));
        }
    }
}