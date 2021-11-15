using System.Collections.Generic;
using Newtonsoft.Json;

namespace BookLovers.Seed.Models.OpenLibrary.Books
{
    internal class BookRoot
    {
        public BookDescription Description { get; set; }

        public string Title { get; set; }

        public List<int?> Covers { get; set; }

        public string Key { get; set; }

        public List<BookAuthor> Authors { get; set; }

        public List<string> Publishers { get; set; }

        [JsonProperty(PropertyName = "number_of_pages")]
        public string Pages { get; set; }

        [JsonProperty(PropertyName = "isbn_10")]
        public string[] Isbn10 { get; set; }

        [JsonProperty(PropertyName = "isbn_13")]
        public string[] Isbn13 { get; set; }

        [JsonProperty(PropertyName = "physical_format")]
        public string PhysicalFormat { get; set; }

        public int LocalId { get; set; }

        [JsonProperty(PropertyName = "publish_date")]
        public string PublishDate { get; set; }

        public List<BookLanguage> Languages { get; set; }

        public BookType Type { get; set; }

        public BookCreated Created { get; set; }

        [JsonProperty(PropertyName = "last_modified")]
        public BookEdit LastModified { get; set; }

        public BookIdentifiers Identifiers { get; set; }
    }
}