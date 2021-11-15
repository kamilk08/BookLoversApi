using System.Collections.Generic;

namespace BookLovers.Seed.Models.Configuration
{
    public class OpenLibrarySeedData
    {
        public IEnumerable<SeedAuthor> Authors { get; set; }

        public IEnumerable<SeedBook> Books { get; set; }

        public IEnumerable<SeedPublisher> Publishers { get; set; }

        public OpenLibrarySeedData(
            IEnumerable<SeedAuthor> authors,
            IEnumerable<SeedBook> books,
            IEnumerable<SeedPublisher> publishers)
        {
            this.Authors = authors;
            this.Books = books;
            this.Publishers = publishers;
        }
    }
}