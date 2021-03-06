using System;
using System.Collections.Generic;
using System.Linq;
using BookLovers.Base.Infrastructure.Events.IntegrationEvents;
using BookLovers.Base.Infrastructure.Extensions;
using BookLovers.Base.Infrastructure.ModuleCommunication;
using BookLovers.Base.Infrastructure.Serialization;
using BookLovers.Bookcases.Infrastructure.Persistence;
using Newtonsoft.Json;

namespace BookLovers.Bookcases.Infrastructure.Root.Outbox
{
    public class OutboxMessagesProcessor
    {
        private readonly BookcaseContext _context;
        private readonly InMemoryEventBus _eventBus;

        public OutboxMessagesProcessor(BookcaseContext context, InMemoryEventBus eventBus)
        {
            _context = context;
            _eventBus = eventBus;
        }

        public void Process()
        {
            var messages = _context.OutboxMessages
                .OrderBy(p => p.OccuredAt)
                .Where(p => p.ProcessedAt == null).ToList();

            foreach (var message in messages)
            {
                var map = JsonConvert.DeserializeObject<Dictionary<int, bool>>(message.Map);
                var instance = ReflectionHelper.CreateInstance(message.Assembly, message.Type);

                _eventBus.PublishWithMap(
                    JsonConvert.DeserializeObject(message.Data, instance.GetType(),
                    SerializerSettings.GetSerializerSettings()) as IIntegrationEvent, map);

                if (!map.Values.All(p => p))
                    return;

                message.Map = JsonConvert.SerializeObject(map);
                message.ProcessedAt = DateTime.UtcNow;
            }

            _context.SaveChanges();
        }
    }
}