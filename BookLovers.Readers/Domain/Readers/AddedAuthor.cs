using System;

namespace BookLovers.Readers.Domain.Readers
{
    public class AddedAuthor : BookLovers.Base.Domain.ValueObject.ValueObject<AddedAuthor>, IAddedResource
    {
        public Guid AuthorGuid { get; }

        public int AuthorId { get; }

        public DateTime AddedAt { get; }

        public Guid ResourceGuid => AuthorGuid;

        public AddedResourceType AddedResourceType => AddedResourceType.Author;

        private AddedAuthor()
        {
        }

        public AddedAuthor(Guid authorGuid, int authorId, DateTime addedAt)
        {
            AuthorGuid = authorGuid;
            AuthorId = authorId;
            AddedAt = addedAt;
        }

        protected override int GetHashCodeCore()
        {
            var hash = 17;

            hash = (hash * 23) + this.AuthorGuid.GetHashCode();
            hash = (hash * 23) + this.AddedAt.GetHashCode();

            return hash;
        }

        protected override bool EqualsCore(AddedAuthor obj) =>
            AuthorGuid == obj.AuthorGuid && AddedAt == obj.AddedAt;
    }
}