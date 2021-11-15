using System;
using BookLovers.Base.Infrastructure.Queries;

namespace BookLovers.Readers.Infrastructure.Persistence.ReadModels
{
    public class AvatarReadModel : IReadModel
    {
        public int Id { get; set; }

        public Guid ReaderGuid { get; set; }

        public int ReaderId { get; set; }

        public string AvatarUrl { get; set; }

        public string FileName { get; set; }

        public string MimeType { get; set; }
    }
}