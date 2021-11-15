using System;
using BookLovers.Base.Infrastructure.Commands;

namespace BookLovers.Readers.Application.Commands.NotificationWalls
{
    internal class AddAuthorDismissedNotificationInternalCommand : IInternalCommand, ICommand
    {
        public Guid Guid { get; private set; }

        public Guid ReaderGuid { get; private set; }

        public Guid DismissedByGuid { get; private set; }

        public Guid AuthorGuid { get; private set; }

        public string Notification { get; private set; }

        private AddAuthorDismissedNotificationInternalCommand()
        {
        }

        public AddAuthorDismissedNotificationInternalCommand(
            Guid readerGuid,
            Guid dismissedByGuid,
            Guid authorGuid,
            string notification)
        {
            ReaderGuid = readerGuid;
            DismissedByGuid = dismissedByGuid;
            AuthorGuid = authorGuid;
            Notification = notification;
        }
    }
}