using System;

namespace BookLovers.Base.Domain
{
    public interface IMemento
    {
    }

    public interface IMemento<in TAggregate> : IMemento
        where TAggregate : class
    {
        int Version { get; }

        int LastCommittedVersion { get; }

        int AggregateStatus { get; }

        Guid AggregateGuid { get; }

        IMemento<TAggregate> TakeSnapshot(TAggregate aggregate);
    }
}