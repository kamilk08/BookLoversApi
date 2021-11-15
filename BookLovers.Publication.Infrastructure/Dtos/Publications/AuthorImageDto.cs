namespace BookLovers.Publication.Infrastructure.Dtos.Publications
{
    public class AuthorImageDto
    {
        public int Id { get; set; }

        public int AuthorId { get; set; }

        public string FileName { get; set; }

        public string MimeType { get; set; }

        public string FileUrl { get; set; }
    }
}