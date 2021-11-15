using System.Collections.Generic;
using BookLovers.Publication.Infrastructure.Dtos.Publications;

namespace BookLovers.Publication.Infrastructure.Dtos
{
    public class SearchDto
    {
        public IList<AuthorDto> Authors { get; set; }

        public IList<BookDto> Books { get; set; }

        public IList<PublisherDto> Publishers { get; set; }

        public IList<SeriesDto> Series { get; set; }

        public SearchDto(
            IList<AuthorDto> authors,
            IList<BookDto> books,
            IList<PublisherDto> publishers,
            IList<SeriesDto> series)
        {
            this.Authors = authors;
            this.Books = books;
            this.Publishers = publishers;
            this.Series = series;
        }
    }
}