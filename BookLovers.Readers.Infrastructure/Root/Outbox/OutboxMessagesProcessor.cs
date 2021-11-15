using System;
using System.Collections.Generic;
using System.Linq;
using BookLovers.Base.Infrastructure.Events.IntegrationEvents;
using BookLovers.Base.Infrastructure.Extensions;
using BookLovers.Base.Infrastructure.ModuleCommunication;
using BookLovers.Base.Infrastructure.Serialization;
using BookLovers.Readers.Infrastructure.Persistence;
using Newtonsoft.Json;

namespace BookLovers.Readers.Infrastructure.Root.Outbox
{
    internal class OutboxMessagesProcessor
    {
        private readonly ReadersContext _context;
        private readonly IInMemoryEventBus _eventBus;

        public OutboxMessagesProcessor(ReadersContext context, IInMemoryEventBus eventBus)
        {
            this._context = context;
            this._eventBus = eventBus;
        }

        public void Process()
        {
            var messages = this._context.OutboxMessages
                .OrderBy(p => p.OccuredAt)
                .Where(p => p.ProcessedAt == null).ToList();

            foreach (var outboxMessage in messages)
            {
                var map = JsonConvert.DeserializeObject<Dictionary<int, bool>>(outboxMessage.Map);

                var instance = ReflectionHelper.CreateInstance(outboxMessage.Assembly, outboxMessage.Type);

                this._eventBus.PublishWithMap(
                    JsonConvert.DeserializeObject(
                        outboxMessage.Data,
                        instance.GetType(),
                        SerializerSettings.GetSerializerSettings()) as IIntegrationEvent, map);

                if (!map.Values.All(p => p))
                    return;

                outboxMessage.Map = JsonConvert.SerializeObject(map);

                outboxMessage.ProcessedAt = new DateTime?(DateTime.UtcNow);
            }

            this._context.SaveChanges();
        }
    }
}