using System;
using BookLovers.Base.Domain.Events;
using Newtonsoft.Json;

namespace BookLovers.Librarians.Events.PromotionWaiters
{
    public class PromotionWaiterStatusChanged : IEvent
    {
        public Guid Guid { get; }

        public Guid AggregateGuid { get; }

        public Guid ReaderGuid { get; }

        public byte CurrentStatus { get; }

        private PromotionWaiterStatusChanged()
        {
        }

        [JsonConstructor]
        protected PromotionWaiterStatusChanged(
            Guid guid,
            Guid aggregateGuid,
            Guid readerGuid,
            byte currentStatus)
        {
            this.Guid = guid;
            this.AggregateGuid = aggregateGuid;
            this.ReaderGuid = readerGuid;
            this.CurrentStatus = currentStatus;
        }

        public PromotionWaiterStatusChanged(Guid aggregateGuid, Guid readerGuid, byte currentStatus)
        {
            this.Guid = Guid.NewGuid();
            this.AggregateGuid = aggregateGuid;
            this.ReaderGuid = readerGuid;
            this.CurrentStatus = currentStatus;
        }
    }
}