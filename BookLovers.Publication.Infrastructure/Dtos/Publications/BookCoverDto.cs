namespace BookLovers.Publication.Infrastructure.Dtos.Publications
{
    public class BookCoverDto
    {
        public int Id { get; set; }

        public int BookId { get; set; }

        public string FileName { get; set; }

        public string MimeType { get; set; }

        public string FileUrl { get; set; }

        public string CoverSource { get; set; }
    }
}