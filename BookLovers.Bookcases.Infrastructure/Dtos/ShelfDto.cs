using System;
using System.Collections.Generic;

namespace BookLovers.Bookcases.Infrastructure.Dtos
{
    public class ShelfDto
    {
        public int Id { get; set; }

        public Guid Guid { get; set; }

        public byte ShelfCategory { get; set; }

        public string ShelfName { get; set; }

        public int BookcaseId { get; set; }

        public IEnumerable<BookDto> Publications { get; set; }

        public int PublicationsCount { get; set; }
    }
}