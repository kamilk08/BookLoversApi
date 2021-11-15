using System.Collections.Generic;
using BookLovers.Base.Infrastructure.ModuleCommunication;

namespace BaseTests.EventBusTests
{
    public class FakeProducerNotification :
        IProducerNotification<FakeIntegrationEvent>,
        IProducerNotification
    {
        public void NotifyProducer(FakeIntegrationEvent @event, Dictionary<int, bool> map)
        {
        }
    }
}