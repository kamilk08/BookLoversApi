using System;
using BookLovers.Base.Domain.ValueObject;

namespace BookLovers.Readers.Domain.Readers
{
    public class AddedBook : ValueObject<AddedBook>, IAddedResource
    {
        public Guid BookGuid { get; }

        public int BookId { get; }

        public DateTime AddedAt { get; }

        public Guid ResourceGuid => BookGuid;

        public AddedResourceType AddedResourceType => AddedResourceType.Book;

        private AddedBook()
        {
        }

        public AddedBook(Guid bookGuid, int bookId, DateTime addedAt)
        {
            BookGuid = bookGuid;
            BookId = bookId;
            AddedAt = addedAt;
        }

        protected override int GetHashCodeCore()
        {
            var hash = 17;

            hash = (hash * 23) + this.BookGuid.GetHashCode();
            hash = (hash * 23) + this.AddedAt.GetHashCode();

            return hash;
        }

        protected override bool EqualsCore(AddedBook obj) =>
            obj.BookGuid == BookGuid && obj.AddedAt == AddedAt;
    }
}