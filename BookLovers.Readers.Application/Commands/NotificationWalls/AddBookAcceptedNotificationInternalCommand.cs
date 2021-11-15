using System;
using BookLovers.Base.Infrastructure.Commands;

namespace BookLovers.Readers.Application.Commands.NotificationWalls
{
    internal class AddBookAcceptedNotificationInternalCommand : ICommand, IInternalCommand
    {
        public Guid ReaderGuid { get; private set; }

        public Guid AcceptedByGuid { get; private set; }

        public Guid BookGuid { get; private set; }

        public string Notification { get; private set; }

        public Guid Guid { get; private set; }

        private AddBookAcceptedNotificationInternalCommand()
        {
        }

        public AddBookAcceptedNotificationInternalCommand(
            Guid readerGuid,
            Guid acceptedByGuid,
            Guid bookGuid,
            string notification)
        {
            Guid = Guid.NewGuid();
            ReaderGuid = readerGuid;
            BookGuid = bookGuid;
            AcceptedByGuid = acceptedByGuid;
            Notification = notification;
        }
    }
}