using System;
using System.Collections.Generic;
using System.Linq;
using BookLovers.Base.Infrastructure.Events.IntegrationEvents;
using BookLovers.Base.Infrastructure.Extensions;
using BookLovers.Base.Infrastructure.ModuleCommunication;
using BookLovers.Base.Infrastructure.Serialization;
using BookLovers.Publication.Infrastructure.Persistence;
using Newtonsoft.Json;

namespace BookLovers.Publication.Infrastructure.Root.Outbox
{
    public class OutboxMessagesProcessor
    {
        private readonly PublicationsContext _context;
        private readonly InMemoryEventBus _eventBus;

        public OutboxMessagesProcessor(PublicationsContext context, InMemoryEventBus eventBus)
        {
            _context = context;
            _eventBus = eventBus;
        }

        public void Process()
        {
            var messages = _context.OutboxMessages.OrderBy(p => p.OccuredAt)
                .Where(p => p.ProcessedAt == null).ToList();

            foreach (var outboxMessage in messages)
            {
                var map = JsonConvert.DeserializeObject<Dictionary<int, bool>>(outboxMessage.Map);

                var instance = ReflectionHelper.CreateInstance(outboxMessage.Assembly, outboxMessage.Type);

                _eventBus.PublishWithMap(
                    JsonConvert.DeserializeObject(outboxMessage.Data, instance.GetType(),
                        SerializerSettings.GetSerializerSettings()) as IIntegrationEvent, map);

                if (!map.Values.All(p => p))
                    return;

                outboxMessage.Map = JsonConvert.SerializeObject(map);
                outboxMessage.ProcessedAt = DateTime.UtcNow;
            }

            _context.SaveChanges();
        }
    }
}