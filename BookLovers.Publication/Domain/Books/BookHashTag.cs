using BookLovers.Base.Domain.ValueObject;

namespace BookLovers.Publication.Domain.Books
{
    public class BookHashTag : ValueObject<BookHashTag>
    {
        public string HashTagContent { get; }

        private BookHashTag()
        {
        }

        public BookHashTag(string hashTagContent)
        {
            this.HashTagContent = hashTagContent;
        }

        protected override int GetHashCodeCore()
        {
            return (17 * 23) + this.HashTagContent.GetHashCode();
        }

        protected override bool EqualsCore(BookHashTag obj)
        {
            return this.HashTagContent == obj.HashTagContent;
        }
    }
}