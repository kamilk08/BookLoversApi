using System;

namespace BookLovers.Publication.Application.WriteModels.Books
{
    public class BookBasicsWriteModel
    {
        public string Isbn { get; set; }

        public string Title { get; set; }

        public DateTime PublicationDate { get; set; }

        public int Category { get; set; }

        public int SubCategory { get; set; }

        public Guid PublisherGuid { get; set; }
    }
}