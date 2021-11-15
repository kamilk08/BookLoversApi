using BookLovers.Base.Domain.ValueObject;
using BookLovers.Publication.Domain.Books.IsbnValidation;

namespace BookLovers.Publication.Domain.Books
{
    public class Isbn : ValueObject<Isbn>
    {
        public string Value { get; }

        public IsbnType Type { get; }

        private Isbn()
        {
        }

        public Isbn(string value)
        {
            this.Value = value;
            this.Type = value.Length == IsbnType.ISBN10.Value ? IsbnType.ISBN10 : IsbnType.ISBN13;
        }

        protected override int GetHashCodeCore()
        {
            var hash = 17;

            hash = (hash * 23) + this.Value.GetHashCode();
            hash = (hash * 23) + this.Type.GetHashCode();

            return hash;
        }

        protected override bool EqualsCore(Isbn obj)
        {
            return this.Value == obj.Value && this.Type == obj.Type;
        }
    }
}