using System;
using System.Collections.Generic;
using BookLovers.Base.Domain;
using BookLovers.Readers.Domain.Reviews;

namespace BookLovers.Readers.Mementos
{
    public interface IReviewMemento : IMemento<Review>, IMemento
    {
        IEnumerable<Guid> Likes { get; }

        IEnumerable<Guid> ReviewReports { get; }

        IEnumerable<Guid> SpoilerTags { get; }

        Guid BookGuid { get; }

        Guid ReaderGuid { get; }

        string Review { get; }

        DateTime CreatedAt { get; }

        DateTime? EditedAt { get; }

        bool MarkedAsSpoilerByReader { get; }

        bool MarkedByOtherReaders { get; }
    }
}