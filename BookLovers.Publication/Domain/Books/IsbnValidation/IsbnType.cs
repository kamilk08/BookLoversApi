using BookLovers.Base.Domain.Extensions;

namespace BookLovers.Publication.Domain.Books.IsbnValidation
{
    public class IsbnType : Enumeration
    {
        public static readonly IsbnType ISBN10 = new IsbnType(1, "ISBN-10", 10);
        public static readonly IsbnType ISBN13 = new IsbnType(2, "ISBN-13", 13);

        public int IsbnNumberLength { get; }

        private IsbnType()
        {
        }

        protected IsbnType(int value, string name, int isbnNumberLength)
            : base(value, name)
        {
            this.IsbnNumberLength = isbnNumberLength;
        }
    }
}