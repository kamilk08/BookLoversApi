using System;
using System.Collections.Generic;
using BookLovers.Base.Domain;
using BookLovers.Publication.Domain.Quotes;

namespace BookLovers.Publication.Mementos
{
    public interface IQuoteMemento : IMemento<Quote>, IMemento
    {
        Guid AuthorGuid { get; }

        Guid BookGuid { get; }

        string QuoteContent { get; }

        Guid AddedByGuid { get; }

        DateTime AddedAt { get; }

        int QuoteTypeId { get; }

        IList<Guid> QuoteLikes { get; }
    }
}