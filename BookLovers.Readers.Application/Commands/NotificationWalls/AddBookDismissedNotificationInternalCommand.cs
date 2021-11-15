using System;
using BookLovers.Base.Infrastructure.Commands;

namespace BookLovers.Readers.Application.Commands.NotificationWalls
{
    internal class AddBookDismissedNotificationInternalCommand : ICommand, IInternalCommand
    {
        public Guid ReaderGuid { get; private set; }

        public Guid DismissedByGuid { get; private set; }

        public Guid BookGuid { get; private set; }

        public string Notification { get; private set; }

        public Guid Guid { get; private set; }

        private AddBookDismissedNotificationInternalCommand()
        {
        }

        public AddBookDismissedNotificationInternalCommand(
            Guid readerGuid,
            Guid dismissedByGuid,
            Guid bookGuid,
            string notification)
        {
            ReaderGuid = readerGuid;
            DismissedByGuid = dismissedByGuid;
            BookGuid = bookGuid;
            Notification = notification;
            Guid = Guid.NewGuid();
        }
    }
}