using System;
using System.Collections.Generic;
using System.Linq;
using BookLovers.Auth.Infrastructure.Persistence;
using BookLovers.Base.Infrastructure.Events.IntegrationEvents;
using BookLovers.Base.Infrastructure.Extensions;
using BookLovers.Base.Infrastructure.ModuleCommunication;
using BookLovers.Base.Infrastructure.Serialization;
using Newtonsoft.Json;

namespace BookLovers.Auth.Infrastructure.Root.Outbox
{
    internal class OutboxMessagesProcessor
    {
        private readonly AuthContext _context;
        private readonly IInMemoryEventBus _eventBus;

        public OutboxMessagesProcessor(AuthContext context, IInMemoryEventBus eventBus)
        {
            _context = context;
            _eventBus = eventBus;
        }

        public void Process()
        {
            var source = _context.OutboxMessages
                .OrderBy(p => p.OccuredAt)
                .Where(p => p.ProcessedAt == null)
                .ToList();

            foreach (var outboxMessage in source)
            {
                var map = JsonConvert.DeserializeObject<Dictionary<int, bool>>(outboxMessage.Map);

                var instance = ReflectionHelper.CreateInstance(outboxMessage.Assembly, outboxMessage.Type);

                _eventBus.PublishWithMap(
                    JsonConvert.DeserializeObject(
                        outboxMessage.Data,
                        instance.GetType(),
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