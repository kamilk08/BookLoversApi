using System;
using BookLovers.Base.Domain.ValueObject;

namespace BookLovers.Ratings.Domain.PublisherCycles
{
    public class PublisherCycleIdentification : ValueObject<PublisherCycleIdentification>
    {
        public Guid CycleGuid { get; private set; }

        public int CycleId { get; private set; }

        private PublisherCycleIdentification()
        {
        }

        public PublisherCycleIdentification(Guid cycleGuid, int cycleId)
        {
            this.CycleGuid = cycleGuid;
            this.CycleId = cycleId;
        }

        protected override int GetHashCodeCore()
        {
            var hash = 17;

            hash = (hash * 23) + this.CycleId.GetHashCode();
            hash = (hash * 23) + this.CycleGuid.GetHashCode();

            return hash;
        }

        protected override bool EqualsCore(PublisherCycleIdentification obj)
        {
            return this.CycleGuid == obj.CycleGuid && this.CycleId == obj.CycleId;
        }
    }
}