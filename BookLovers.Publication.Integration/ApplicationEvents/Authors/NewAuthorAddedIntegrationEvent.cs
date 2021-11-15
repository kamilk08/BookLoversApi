using System;
using BookLovers.Base.Domain.Aggregates;
using BookLovers.Base.Infrastructure.Events.IntegrationEvents;

namespace BookLovers.Publication.Integration.ApplicationEvents.Authors
{
    public class NewAuthorAddedIntegrationEvent : IIntegrationEvent
    {
        public Guid AuthorGuid { get; private set; }

        public Guid ReaderGuid { get; private set; }

        public int AuthorId { get; private set; }

        public int AuthorStatus { get; private set; }

        public Guid Guid { get; private set; }

        public DateTime OccuredOn { get; private set; }

        private NewAuthorAddedIntegrationEvent()
        {
        }

        public NewAuthorAddedIntegrationEvent(Guid authorGuid, int authorId, Guid readerGuid)
        {
            this.Guid = Guid.NewGuid();
            this.OccuredOn = DateTime.UtcNow;
            this.AuthorGuid = authorGuid;
            this.AuthorId = authorId;
            this.ReaderGuid = readerGuid;
            this.AuthorStatus = AggregateStatus.Active.Value;
        }
    }
}