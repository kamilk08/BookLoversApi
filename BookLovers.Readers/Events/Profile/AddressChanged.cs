using System;
using BookLovers.Base.Domain.Events;
using Newtonsoft.Json;

namespace BookLovers.Readers.Events.Profile
{
    public class AddressChanged : IEvent
    {
        public Guid Guid { get; private set; }

        public Guid AggregateGuid { get; private set; }

        public string Country { get; private set; }

        public string City { get; private set; }

        private AddressChanged()
        {
        }

        [JsonConstructor]
        protected AddressChanged(Guid guid, Guid aggregateGuid, string country, string city)
        {
            Guid = guid;
            AggregateGuid = aggregateGuid;
            Country = country;
            City = city;
        }

        public AddressChanged(Guid aggregateGuid, string country, string city)
        {
            Guid = Guid.NewGuid();
            AggregateGuid = aggregateGuid;
            Country = country;
            City = city;
        }
    }
}