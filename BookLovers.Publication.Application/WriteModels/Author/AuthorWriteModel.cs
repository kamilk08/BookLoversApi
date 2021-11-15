using System;
using System.Collections.Generic;

namespace BookLovers.Publication.Application.WriteModels.Author
{
    public class AuthorWriteModel
    {
        public int AuthorId { get; set; }

        public Guid AuthorGuid { get; set; }

        public AuthorBasicsWriteModel Basics { get; set; }

        public AuthorDetailsWriteModel Details { get; set; }

        public AuthorDescriptionWriteModel Description { get; set; }

        public List<Guid> AuthorBooks { get; set; }

        public List<int> AuthorGenres { get; set; }

        public Guid ReaderGuid { get; set; }
    }
}