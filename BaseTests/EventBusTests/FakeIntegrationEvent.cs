using System;
using BookLovers.Base.Infrastructure.Events.IntegrationEvents;

namespace BaseTests.EventBusTests
{
    public class FakeIntegrationEvent : IIntegrationEvent
    {
        public Guid Guid { get; }

        public DateTime OccuredOn { get; }

        private FakeIntegrationEvent()
        {
        }

        public FakeIntegrationEvent(Guid guid, DateTime occuredOn)
        {
            this.Guid = guid;
            this.OccuredOn = occuredOn;
        }
    }
}