using System;
using BookLovers.Base.Domain.Aggregates;
using BookLovers.Base.Domain.Events;

namespace BookLovers.Bookcases.Events.Settings
{
    public class SettingsManagerArchived : IEvent
    {
        public Guid Guid { get; private set; }

        public Guid AggregateGuid { get; private set; }

        public int Status { get; private set; }

        private SettingsManagerArchived()
        {
        }

        public SettingsManagerArchived(Guid aggregateGuid)
        {
            Guid = Guid.NewGuid();
            AggregateGuid = aggregateGuid;
            Status = AggregateStatus.Archived.Value;
        }
    }
}