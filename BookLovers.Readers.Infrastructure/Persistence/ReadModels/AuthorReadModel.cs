using System;

namespace BookLovers.Readers.Infrastructure.Persistence.ReadModels
{
    public class AuthorReadModel
    {
        public int Id { get; set; }

        public Guid AuthorGuid { get; set; }

        public int AuthorId { get; set; }
    }
}