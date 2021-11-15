using System;

namespace BookLovers.Publication.Domain.Publishers
{
    public class Cycle : BookLovers.Base.Domain.ValueObject.ValueObject<Cycle>
    {
        public Guid CycleGuid { get; }

        private Cycle()
        {
        }

        public Cycle(Guid cycleGuid) => this.CycleGuid = cycleGuid;

        protected override int GetHashCodeCore() =>
            (17 * 23) + this.CycleGuid.GetHashCode();

        protected override bool EqualsCore(Cycle obj) =>
            this.CycleGuid == obj.CycleGuid;
    }
}