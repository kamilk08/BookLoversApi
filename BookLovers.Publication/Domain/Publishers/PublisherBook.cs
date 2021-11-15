using System;
using BookLovers.Base.Domain.ValueObject;

namespace BookLovers.Publication.Domain.Publishers
{
    public class PublisherBook : ValueObject<PublisherBook>
    {
        public Guid BookGuid { get; }

        private PublisherBook()
        {
        }

        public PublisherBook(Guid bookGuid)
        {
            this.BookGuid = bookGuid;
        }

        protected override int GetHashCodeCore()
        {
            return (17 * 23) + this.BookGuid.GetHashCode();
        }

        protected override bool EqualsCore(PublisherBook obj) =>
            this.BookGuid == obj.BookGuid;
    }
}