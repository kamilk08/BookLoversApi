using System;
using BookLovers.Base.Infrastructure.Commands;

namespace BookLovers.Readers.Application.Commands.NotificationWalls
{
    internal class AddReviewReportedNotificationInternalCommand : IInternalCommand, ICommand
    {
        public Guid Guid { get; }

        public Guid ReaderGuid { get; }

        public Guid ReportedByGuid { get; }

        private AddReviewReportedNotificationInternalCommand()
        {
        }

        public AddReviewReportedNotificationInternalCommand(Guid readerGuid, Guid reportedByGuid)
        {
            Guid = Guid.NewGuid();
            ReaderGuid = readerGuid;
            ReportedByGuid = reportedByGuid;
        }
    }
}