using System;
using BookLovers.Base.Domain.Events;

namespace BookLovers.Bookcases.Events.Settings
{
    public class PrivacyOptionChanged : IEvent
    {
        public Guid Guid { get; private set; }

        public Guid AggregateGuid { get; private set; }

        public Guid BookcaseGuid { get; private set; }

        public int Privacy { get; private set; }

        private PrivacyOptionChanged()
        {
        }

        public PrivacyOptionChanged(Guid aggregateGuid, Guid bookcaseGuid, int privacy)
        {
            Guid = Guid.NewGuid();
            AggregateGuid = aggregateGuid;
            BookcaseGuid = bookcaseGuid;
            Privacy = privacy;
        }
    }
}