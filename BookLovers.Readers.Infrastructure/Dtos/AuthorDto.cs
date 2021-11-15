using System;

namespace BookLovers.Readers.Infrastructure.Dtos
{
    public class AuthorDto
    {
        public int Id { get; set; }

        public Guid AuthorGuid { get; set; }

        public int AuthorId { get; set; }
    }
}