using System;
using BookLovers.Base.Infrastructure.Events.IntegrationEvents;

namespace BookLovers.Librarians.Integration.IntegrationEvents
{
    public class BookDismissedByLibrarian : IIntegrationEvent
    {
        public Guid Guid { get; private set; }

        public DateTime OccuredOn { get; private set; }

        public Guid ReaderGuid { get; private set; }

        public Guid DismissedByGuid { get; private set; }

        public Guid BookGuid { get; private set; }

        public string Justification { get; private set; }

        private BookDismissedByLibrarian()
        {
        }

        private BookDismissedByLibrarian(
            Guid readerGuid,
            Guid dismissedByGuid,
            Guid bookGuid,
            string justification)
        {
            this.Guid = Guid.NewGuid();
            this.OccuredOn = DateTime.UtcNow;
            this.ReaderGuid = readerGuid;
            this.DismissedByGuid = dismissedByGuid;
            this.BookGuid = bookGuid;
            this.Justification = justification;
        }

        public static BookDismissedByLibrarian Initialize()
        {
            return new BookDismissedByLibrarian();
        }

        public BookDismissedByLibrarian WithReader(Guid aggregateGuid)
        {
            return new BookDismissedByLibrarian(aggregateGuid, this.DismissedByGuid, this.BookGuid, this.Justification);
        }

        public BookDismissedByLibrarian DismissedBy(Guid librarianGuid)
        {
            return new BookDismissedByLibrarian(this.ReaderGuid, librarianGuid, this.BookGuid, this.Justification);
        }

        public BookDismissedByLibrarian WithBook(Guid bookGuid)
        {
            return new BookDismissedByLibrarian(this.ReaderGuid, this.DismissedByGuid, bookGuid, this.Justification);
        }

        public BookDismissedByLibrarian WithNotification(string notification)
        {
            return new BookDismissedByLibrarian(this.ReaderGuid, this.DismissedByGuid, this.BookGuid, notification);
        }
    }
}