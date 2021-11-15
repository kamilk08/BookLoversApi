using System;
using BookLovers.Base.Infrastructure.Commands;

namespace BookLovers.Readers.Application.Commands.NotificationWalls
{
    internal class AddAuthorAcceptedNotificationInternalCommand : IInternalCommand, ICommand
    {
        public Guid ReaderGuid { get; private set; }

        public Guid AcceptedByGuid { get; private set; }

        public Guid AuthorGuid { get; private set; }

        public string Notification { get; private set; }

        public Guid Guid { get; private set; }

        private AddAuthorAcceptedNotificationInternalCommand()
        {
        }

        public AddAuthorAcceptedNotificationInternalCommand(
            Guid readerGuid,
            Guid acceptedByGuid,
            Guid authorGuid,
            string notification)
        {
            Guid = Guid.NewGuid();
            ReaderGuid = readerGuid;
            AcceptedByGuid = acceptedByGuid;
            AuthorGuid = authorGuid;
            Notification = notification;
        }
    }
}