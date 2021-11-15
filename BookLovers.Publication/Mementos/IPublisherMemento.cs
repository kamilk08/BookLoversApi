using System;
using System.Collections.Generic;
using BookLovers.Base.Domain;
using BookLovers.Publication.Domain.Publishers;

namespace BookLovers.Publication.Mementos
{
    public interface IPublisherMemento : IMemento<Publisher>, IMemento
    {
        string PublisherName { get; }

        IEnumerable<Guid> Books { get; }

        IEnumerable<Guid> Cycles { get; }
    }
}