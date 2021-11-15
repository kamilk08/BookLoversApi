using System;
using System.Collections.Generic;
using BookLovers.Base.Domain.Events;

namespace BookLovers.Base.Domain.Aggregates
{
    public interface IRoot
    {
        Guid Guid { get; }

        IEnumerable<IEvent> GetUncommittedEvents();

        void CommitEvents();
    }
}