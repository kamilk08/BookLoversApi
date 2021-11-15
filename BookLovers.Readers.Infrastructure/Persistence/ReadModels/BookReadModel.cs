using System;

namespace BookLovers.Readers.Infrastructure.Persistence.ReadModels
{
    public class BookReadModel
    {
        public int Id { get; set; }

        public Guid BookGuid { get; set; }

        public int BookId { get; set; }
    }
}