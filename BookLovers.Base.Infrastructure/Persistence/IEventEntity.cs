using System;

namespace BookLovers.Base.Infrastructure.Persistence
{
    public interface IEventEntity
    {
        int Id { get; set; }

        Guid AggregateGuid { get; set; }

        string Data { get; set; }

        string Type { get; set; }

        string Assembly { get; set; }

        int Version { get; set; }
    }
}