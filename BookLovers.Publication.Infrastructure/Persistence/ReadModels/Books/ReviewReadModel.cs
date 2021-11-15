using System;

namespace BookLovers.Publication.Infrastructure.Persistence.ReadModels.Books
{
    public class ReviewReadModel
    {
        public int Id { get; set; }

        public Guid Guid { get; set; }

        public ReaderReadModel Reader { get; set; }

        public int ReaderId { get; set; }
    }
}