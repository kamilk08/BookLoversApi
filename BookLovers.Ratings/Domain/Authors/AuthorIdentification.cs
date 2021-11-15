using System;
using BookLovers.Base.Domain.ValueObject;

namespace BookLovers.Ratings.Domain.Authors
{
    public class AuthorIdentification : ValueObject<AuthorIdentification>
    {
        public Guid AuthorGuid { get; private set; }

        public int AuthorId { get; private set; }

        private AuthorIdentification()
        {
        }

        public AuthorIdentification(Guid authorGuid, int authorId)
        {
            this.AuthorGuid = authorGuid;
            this.AuthorId = authorId;
        }

        protected override int GetHashCodeCore()
        {
            var hash = 17;

            hash = (hash * 23) + this.AuthorGuid.GetHashCode();
            hash = (hash * 23) + this.AuthorId.GetHashCode();

            return hash;
        }

        protected override bool EqualsCore(AuthorIdentification obj)
        {
            return this.AuthorGuid == obj.AuthorGuid && this.AuthorId == obj.AuthorId;
        }
    }
}