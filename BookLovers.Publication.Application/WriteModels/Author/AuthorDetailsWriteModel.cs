using System;

namespace BookLovers.Publication.Application.WriteModels.Author
{
    public class AuthorDetailsWriteModel
    {
        public DateTime? BirthDate { get; set; }

        public DateTime? DeathDate { get; set; }

        public string BirthPlace { get; set; }
    }
}