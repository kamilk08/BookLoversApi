using System;
using BookLovers.Base.Domain.Events;
using Newtonsoft.Json;

namespace BookLovers.Readers.Events.Profile
{
    public class CurrentRoleChanged : IEvent
    {
        public Guid Guid { get; private set; }

        public Guid AggregateGuid { get; private set; }

        public string CurrentRole { get; private set; }

        private CurrentRoleChanged()
        {
        }

        [JsonConstructor]
        protected CurrentRoleChanged(Guid guid, Guid aggregateGuid, string currentRole)
        {
            Guid = guid;
            AggregateGuid = aggregateGuid;
            CurrentRole = currentRole;
        }

        public CurrentRoleChanged(Guid aggregateGuid, string currentRole)
        {
            AggregateGuid = aggregateGuid;
            CurrentRole = currentRole;
        }
    }
}