using System;

namespace BookLovers.Publication.Domain.PublisherCycles
{
    public interface IPublisherCycleUniquenessChecker
    {
        bool IsUnique(Guid guid);
    }
}