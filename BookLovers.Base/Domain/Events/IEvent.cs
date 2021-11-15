using System;

namespace BookLovers.Base.Domain.Events
{
    public interface IEvent
    {
        Guid Guid { get; }

        Guid AggregateGuid { get; }
    }
}