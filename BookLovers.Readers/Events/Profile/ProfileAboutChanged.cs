using System;
using BookLovers.Base.Domain.Events;
using Newtonsoft.Json;

namespace BookLovers.Readers.Events.Profile
{
    public class ProfileAboutChanged : IEvent
    {
        public Guid Guid { get; private set; }

        public Guid AggregateGuid { get; private set; }

        public string WebSite { get; private set; }

        public string About { get; private set; }

        private ProfileAboutChanged()
        {
        }

        [JsonConstructor]
        protected ProfileAboutChanged(Guid guid, Guid aggregateGuid, string webSite, string about)
        {
            Guid = guid;
            AggregateGuid = aggregateGuid;
            WebSite = webSite;
            About = about;
        }

        public ProfileAboutChanged(Guid aggregateGuid, string webSite, string about)
        {
            Guid = Guid.NewGuid();
            AggregateGuid = aggregateGuid;
            WebSite = webSite;
            About = about;
        }
    }
}