using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BookLovers.Base.Infrastructure.Events.IntegrationEvents;
using BookLovers.Base.Infrastructure.Messages;
using BookLovers.Base.Infrastructure.ModuleCommunication;
using BookLovers.Base.Infrastructure.Serialization;
using Newtonsoft.Json;
using NUnit.Framework;

namespace BaseTests.EventBusTests
{
    [TestFixture]
    public class InMemoryEventBusTests
    {
        private readonly IInMemoryEventBus _eventBus = new InMemoryEventBus();

        [Test]
        public async Task Publish_WhenCalled_ShouldPublish()
        {
            await this._eventBus.Publish<FakeIntegrationEvent>(
                new FakeIntegrationEvent(Guid.NewGuid(), DateTime.UtcNow));
        }

        [Test]
        public void PublishWithMap_WhenCalled_ShouldPublish()
        {
            var map = new Dictionary<int, bool>()
            {
                { 0, true }, { 1, false }
            };

            this._eventBus.PublishWithMap<FakeIntegrationEvent>(
                new FakeIntegrationEvent(Guid.NewGuid(), DateTime.UtcNow), map);
        }

        [Test]
        public void PublishWithMap_WhenCalledAndEventIsGeneric_ShouldPublish()
        {
            var integrationEvent = new FakeIntegrationEvent(Guid.NewGuid(), DateTime.UtcNow);
            var outboxMessage = new OutboxMessage();
            outboxMessage.Data = JsonConvert.SerializeObject(integrationEvent);

            var map = new Dictionary<int, bool>()
            {
                { 0, true }, { 1, false }
            };

            this._eventBus.PublishWithMap<IIntegrationEvent>(
                JsonConvert.DeserializeObject(
                    outboxMessage.Data,
                    integrationEvent.GetType(),
                    SerializerSettings.GetSerializerSettings()) as IIntegrationEvent, map);
        }

        [SetUp]
        public void BeforeEach()
        {
            this._eventBus.Subscribe<FakeIntegrationEvent>(
                new FirstIntegrationEventHandler());

            this._eventBus.Subscribe<FakeIntegrationEvent>(
                new ThirdIntegrationEventHandler());

            this._eventBus.AddNotifier<FakeIntegrationEvent>(
                new FakeProducerNotification());
        }
    }
}