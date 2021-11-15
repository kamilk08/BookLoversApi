using System;
using System.Collections.Generic;
using BookLovers.Base.Domain;
using BookLovers.Publication.Domain.SeriesCycle;

namespace BookLovers.Publication.Mementos
{
    public interface ISeriesMemento : IMemento<Series>, IMemento
    {
        string SeriesName { get; }

        IEnumerable<KeyValuePair<int, Guid>> SeriesBooks { get; }
    }
}