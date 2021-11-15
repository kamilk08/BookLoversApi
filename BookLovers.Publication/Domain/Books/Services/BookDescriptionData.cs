namespace BookLovers.Publication.Domain.Books.Services
{
    public class BookDescriptionData
    {
        public string Content { get; }

        public string DescriptionSource { get; }

        public BookDescriptionData(string content, string descriptionSource)
        {
            this.Content = content;
            this.DescriptionSource = descriptionSource;
        }
    }
}