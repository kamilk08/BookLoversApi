using System;

namespace BookLovers.Publication.Infrastructure.Persistence.ReadModels.Authors
{
    public class AuthorImageReadModel
    {
        public int Id { get; set; }

        public string AuthorPictureUrl { get; set; }

        public string FileName { get; set; }

        public string MimeType { get; set; }

        public Guid AuthorGuid { get; set; }
    }
}