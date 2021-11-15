using System;
using System.Collections.Generic;

namespace BookLovers.Bookcases.Infrastructure.Dtos
{
    public class BookcaseDto
    {
        public int Id { get; set; }

        public Guid Guid { get; set; }

        public Guid ReaderGuid { get; set; }

        public int ReaderId { get; set; }

        public int ShelvesCount { get; set; }

        public int BooksCount { get; set; }

        public BookcaseOptionsDto BookcaseOptions { get; set; }

        public IEnumerable<ShelfDto> Shelves { get; set; }
    }
}