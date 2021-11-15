using System;
using BookLovers.Base.Domain.ValueObject;

namespace BookLovers.Publication.Domain.Books
{
    public class BookPublisher : ValueObject<BookPublisher>
    {
        public Guid PublisherGuid { get; }

        private BookPublisher()
        {
        }

        public BookPublisher(Guid publisherGuid)
        {
            this.PublisherGuid = publisherGuid;
        }

        protected override int GetHashCodeCore()
        {
            return 17 * this.PublisherGuid.GetHashCode();
        }

        protected override bool EqualsCore(BookPublisher obj)
        {
            return this.PublisherGuid == obj.PublisherGuid;
        }
    }
}