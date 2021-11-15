using BookLovers.Base.Domain.ValueObject;

namespace BookLovers.Publication.Domain.Books
{
    public class Description : ValueObject<Description>
    {
        public string BookDescription { get; }

        public string DescriptionSource { get; }

        private Description()
        {
        }

        public Description(string description, string descriptionSource)
        {
            this.BookDescription = description;
            this.DescriptionSource = descriptionSource;
        }

        protected override bool EqualsCore(Description obj)
        {
            return this.BookDescription == obj.BookDescription
                   && this.DescriptionSource == obj.DescriptionSource;
        }

        protected override int GetHashCodeCore()
        {
            var hash = 17;

            hash = (hash * 23) + this.BookDescription.GetHashCode();
            hash = (hash * 23) + this.DescriptionSource.GetHashCode();

            return hash;
        }
    }
}