namespace BookLovers.Publication.Domain.Authors.Services
{
    public class AuthorDescriptionData
    {
        public string AboutAuthor { get; }

        public string DescriptionSource { get; }

        public string WebSite { get; }

        internal AuthorDescriptionData(string aboutAuthor, string descriptionSource, string webSite)
        {
            this.AboutAuthor = aboutAuthor;
            this.DescriptionSource = descriptionSource;
            this.WebSite = webSite;
        }
    }
}