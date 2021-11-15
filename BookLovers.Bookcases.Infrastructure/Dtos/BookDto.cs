using System;

namespace BookLovers.Bookcases.Infrastructure.Dtos
{
    public class BookDto
    {
        public int Id { get; set; }

        public Guid BookGuid { get; set; }
    }
}