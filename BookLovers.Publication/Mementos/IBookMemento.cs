using System;
using System.Collections.Generic;
using BookLovers.Base.Domain;
using BookLovers.Publication.Domain.Books;

namespace BookLovers.Publication.Mementos
{
    public interface IBookMemento : IMemento<Book>, IMemento
    {
        IEnumerable<Guid> Authors { get; }

        IEnumerable<KeyValuePair<Guid, Guid>> BookReviews { get; }

        IEnumerable<string> BookHashTags { get; }

        IEnumerable<Guid> Quotes { get; }

        Guid PublisherGuid { get; }

        Guid? SeriesGuid { get; }

        int? PositionInSeries { get; }

        string Title { get; }

        string Isbn { get; }

        DateTime PublicationDate { get; }

        int BookCategory { get; }

        int BookSubCategory { get; }

        string Description { get; }

        string DescriptionSource { get; }

        string CoverSource { get; }

        int CoverType { get; }

        int Pages { get; }

        int Language { get; }
    }
}