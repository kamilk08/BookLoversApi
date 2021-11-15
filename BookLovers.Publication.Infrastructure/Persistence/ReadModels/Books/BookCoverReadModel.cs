using System;

namespace BookLovers.Publication.Infrastructure.Persistence.ReadModels.Books
{
    public class BookCoverReadModel
    {
        public int Id { get; set; }

        public string FileName { get; set; }

        public string MimeType { get; set; }

        public string CoverUrl { get; set; }

        public Guid BookGuid { get; set; }
    }
}