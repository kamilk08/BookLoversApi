using System;
using System.Collections.Generic;

namespace BookLovers.Librarians.Infrastructure.Dtos
{
    public class LibrarianDto
    {
        public int Id { get; set; }

        public Guid Guid { get; set; }

        public Guid ReaderGuid { get; set; }

        public int ReaderId { get; set; }

        public IList<int> ManagedTickets { get; set; }
    }
}