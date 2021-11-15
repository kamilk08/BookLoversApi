using System;
using BookLovers.Base.Domain.ValueObject;

namespace BookLovers.Publication.Domain.Authors
{
    public class AuthorBook : ValueObject<AuthorBook>
    {
        public Guid BookGuid { get; }

        private AuthorBook()
        {
        }

        public AuthorBook(Guid bookGuid) => this.BookGuid = bookGuid;

        protected override int GetHashCodeCore()
        {
            return (17 * 23) + this.BookGuid.GetHashCode();
        }

        protected override bool EqualsCore(AuthorBook obj) =>
            this.BookGuid == obj.BookGuid;
    }
}