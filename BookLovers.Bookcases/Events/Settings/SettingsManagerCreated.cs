using System;
using BookLovers.Base.Domain.Aggregates;
using BookLovers.Base.Domain.Events;

namespace BookLovers.Bookcases.Events.Settings
{
    public class SettingsManagerCreated : IEvent
    {
        public Guid Guid { get; private set; }

        public Guid AggregateGuid { get; private set; }

        public Guid BookcaseGuid { get; private set; }

        public int Capacity { get; private set; }

        public int Privacy { get; private set; }

        public int Status { get; private set; }

        private SettingsManagerCreated()
        {
        }

        private SettingsManagerCreated(
            Guid aggregateGuid,
            Guid bookcaseGuid,
            int capacity,
            int privacy)
        {
            Guid = Guid.NewGuid();
            AggregateGuid = aggregateGuid;
            BookcaseGuid = bookcaseGuid;
            Capacity = capacity;
            Privacy = privacy;
            Status = AggregateStatus.Active.Value;
        }

        public static SettingsManagerCreated Initialize()
        {
            return new SettingsManagerCreated();
        }

        public SettingsManagerCreated WithAggregate(Guid settingsManagerGuid)
        {
            return new SettingsManagerCreated(settingsManagerGuid, BookcaseGuid, Capacity, Privacy);
        }

        public SettingsManagerCreated WithBookcase(Guid bookcaseGuid)
        {
            return new SettingsManagerCreated(AggregateGuid, bookcaseGuid, Capacity, Privacy);
        }

        public SettingsManagerCreated WithCapacityAndPrivacy(int capacity, int privacy)
        {
            return new SettingsManagerCreated(AggregateGuid, BookcaseGuid, capacity, privacy);
        }
    }
}