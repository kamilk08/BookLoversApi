using System;

namespace BookLovers.Publication.Domain.SeriesCycle.Services
{
    public interface ISeriesUniquenessChecker
    {
        bool IsUnique(Guid guid, string name);
    }
}