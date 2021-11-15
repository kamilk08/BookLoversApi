using System.Threading.Tasks;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Base.Infrastructure.Events.DomainEvents;
using BookLovers.Readers.Application.Commands.NotificationWalls;
using BookLovers.Readers.Events.Readers;

namespace BookLovers.Readers.Application.Events.NotificationsWall.Followers
{
    internal class ReaderFollowedEventHandler :
        IDomainEventHandler<ReaderFollowed>,
        IDomainEventHandler
    {
        private readonly IInternalCommandDispatcher _dispatcher;

        public ReaderFollowedEventHandler(
            IInternalCommandDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        public Task HandleAsync(ReaderFollowed @event)
        {
            return _dispatcher.SendInternalCommandAsync(
                new AddNewFollowerNotificationInternalCommand(@event.NotificationWallGuid, @event.FollowedByGuid));
        }
    }
}