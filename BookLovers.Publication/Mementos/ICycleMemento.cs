using System;
using System.Collections.Generic;
using BookLovers.Base.Domain;
using BookLovers.Publication.Domain.PublisherCycles;

namespace BookLovers.Publication.Mementos
{
    public interface ICycleMemento : IMemento<PublisherCycle>, IMemento
    {
        string CycleName { get; }

        Guid PublisherGuid { get; }

        IEnumerable<Guid> CycleBooks { get; }
    }
}