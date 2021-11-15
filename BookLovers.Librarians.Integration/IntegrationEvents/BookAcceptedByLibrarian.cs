using System;
using BookLovers.Base.Infrastructure.Events.IntegrationEvents;

namespace BookLovers.Librarians.Integration.IntegrationEvents
{
    public class BookAcceptedByLibrarian : IIntegrationEvent
    {
        public Guid Guid { get; private set; }

        public DateTime OccuredOn { get; private set; }

        public Guid ReaderGuid { get; private set; }

        public Guid AcceptedByGuid { get; private set; }

        public Guid BookGuid { get; private set; }

        public string Notification { get; private set; }

        public string BookData { get; private set; }

        private BookAcceptedByLibrarian()
        {
        }

        private BookAcceptedByLibrarian(
            Guid readerGuid,
            Guid acceptedByGuid,
            Guid bookGuid,
            string bookData,
            string notification)
        {
            this.Guid = Guid.NewGuid();
            this.OccuredOn = DateTime.UtcNow;
            this.ReaderGuid = readerGuid;
            this.AcceptedByGuid = acceptedByGuid;
            this.BookGuid = bookGuid;
            this.BookData = bookData;
            this.Notification = notification;
        }

        public static BookAcceptedByLibrarian Initialize()
        {
            return new BookAcceptedByLibrarian();
        }

        public BookAcceptedByLibrarian WithReader(Guid readerGuid)
        {
            return new BookAcceptedByLibrarian(readerGuid, this.AcceptedByGuid, this.BookGuid, this.BookData,
                this.Notification);
        }

        public BookAcceptedByLibrarian AcceptedBy(Guid librarianGuid)
        {
            return new BookAcceptedByLibrarian(this.ReaderGuid, librarianGuid, this.BookGuid, this.BookData,
                this.Notification);
        }

        public BookAcceptedByLibrarian WithBook(Guid bookGuid, string bookData)
        {
            return new BookAcceptedByLibrarian(this.ReaderGuid, this.AcceptedByGuid, bookGuid, bookData,
                this.Notification);
        }

        public BookAcceptedByLibrarian WithNotification(string notification)
        {
            return new BookAcceptedByLibrarian(this.ReaderGuid, this.AcceptedByGuid, this.BookGuid, this.BookData,
                notification);
        }
    }
}