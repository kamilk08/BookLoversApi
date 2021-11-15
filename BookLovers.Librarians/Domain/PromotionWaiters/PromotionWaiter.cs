using System;
using BookLovers.Base.Domain.Aggregates;
using BookLovers.Librarians.Domain.PromotionWaiters.BusinessRules;

namespace BookLovers.Librarians.Domain.PromotionWaiters
{
    public class PromotionWaiter : AggregateRoot
    {
        public Guid ReaderGuid { get; private set; }

        public int ReaderId { get; private set; }

        public PromotionAvailability PromotionAvailability { get; private set; }

        private PromotionWaiter()
        {
        }

        public PromotionWaiter(Guid aggregateGuid, Guid readerGuid, int readerId)
        {
            this.Guid = aggregateGuid;
            this.ReaderGuid = readerGuid;
            this.ReaderId = readerId;
            this.PromotionAvailability = PromotionAvailability.Available;
            this.Status = AggregateStatus.Active.Value;
        }

        public PromotionWaiter(
            Guid aggregateGuid,
            Guid readerGuid,
            int readerId,
            PromotionAvailability promotionAvailability)
        {
            this.Guid = aggregateGuid;
            this.ReaderGuid = readerGuid;
            this.ReaderId = readerId;
            this.PromotionAvailability = promotionAvailability;
            this.Status = AggregateStatus.Active.Value;
        }

        public void ChangeAvailability(PromotionAvailability availability)
        {
            this.CheckBusinessRules(new ChangeAvailabilityRules(this));

            this.PromotionAvailability = availability;
        }
    }
}