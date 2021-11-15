using System;

namespace BookLovers.Publication.Infrastructure.Persistence.ReadModels.Quotes
{
    public class QuoteLikeReadModel
    {
        public int Id { get; set; }

        public Guid ReaderGuid { get; set; }

        public int ReaderId { get; set; }
    }
}