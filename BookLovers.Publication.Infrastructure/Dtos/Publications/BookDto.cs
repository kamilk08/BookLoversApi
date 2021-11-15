using System;
using System.Collections.Generic;

namespace BookLovers.Publication.Infrastructure.Dtos.Publications
{
    public class BookDto
    {
        public int Id { get; set; }

        public Guid Guid { get; set; }

        public string Isbn { get; set; }

        public string Title { get; set; }

        public int Pages { get; set; }

        public IList<AuthorDto> Authors { get; set; }

        public int CategoryId { get; set; }

        public byte SubCategoryId { get; set; }

        public string SubCategoryName { get; set; }

        public string CategoryName { get; set; }

        public DateTime PublicationDate { get; set; }

        public string Description { get; set; }

        public string DescriptionSource { get; set; }

        public SeriesDto Series { get; set; }

        public byte? PositionInSeries { get; set; }

        public IEnumerable<PublisherCycleDto> Cycles { get; set; }

        public PublisherDto Publisher { get; set; }

        public List<ReviewDto> Reviews { get; set; }

        public byte LanguageId { get; set; }

        public string Language { get; set; }

        public int ReaderId { get; set; }

        public Guid ReaderGuid { get; set; }

        public byte BookStatus { get; set; }

        public string CoverSource { get; set; }

        public byte CoverTypeId { get; set; }

        public bool IsCoverAdded { get; set; }

        public IList<string> BookHashTags { get; set; }
    }
}