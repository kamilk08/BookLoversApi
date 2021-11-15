using BookLovers.Base.Domain.ValueObject;

namespace BookLovers.Publication.Domain.Authors
{
    public class AuthorDescription : ValueObject<AuthorDescription>
    {
        public string AboutAuthor { get; }

        public string AuthorWebsite { get; }

        public string DescriptionSource { get; }

        private AuthorDescription()
        {
        }

        public AuthorDescription(string description, string webSite, string descriptionSource)
        {
            this.AboutAuthor = description;
            this.DescriptionSource = descriptionSource;
            this.AuthorWebsite = webSite;
        }

        public static AuthorDescription Default()
        {
            return new AuthorDescription(string.Empty, string.Empty, string.Empty);
        }

        protected override int GetHashCodeCore()
        {
            var hash = 17;

            hash = (hash * 23) + this.AboutAuthor.GetHashCode();
            hash = (hash * 23) + this.DescriptionSource.GetHashCode();

            return hash;
        }

        protected override bool EqualsCore(AuthorDescription obj)
        {
            return this.AboutAuthor == obj.AboutAuthor
                   && this.DescriptionSource == obj.DescriptionSource;
        }
    }
}