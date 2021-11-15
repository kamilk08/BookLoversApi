using System;

namespace BookLovers.Librarians.Application.WriteModels
{
    public class CreateLibrarianWriteModel
    {
        public int LibrarianId { get; set; }

        public Guid ReaderGuid { get; set; }

        public Guid LibrarianGuid { get; set; }
    }
}