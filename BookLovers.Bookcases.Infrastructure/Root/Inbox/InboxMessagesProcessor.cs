using System;
using System.Linq;
using System.Threading.Tasks;
using BookLovers.Base.Infrastructure.Events.IntegrationEvents;
using BookLovers.Base.Infrastructure.Extensions;
using BookLovers.Base.Infrastructure.Serialization;
using BookLovers.Bookcases.Infrastructure.Persistence;
using Newtonsoft.Json;

namespace BookLovers.Bookcases.Infrastructure.Root.Inbox
{
    internal class InboxMessagesProcessor
    {
        private readonly BookcaseContext _context;
        private readonly IIntegrationEventDispatcher _dispatcher;

        public InboxMessagesProcessor(BookcaseContext context, IIntegrationEventDispatcher dispatcher)
        {
            _context = context;
            _dispatcher = dispatcher;
        }

        public void Process()
        {
            var messages = _context.InBoxMessages.OrderBy(p => p.OccurredOn)
                .Where(p => p.ProcessedAt == null).ToList();

            foreach (var message in messages)
            {
                var @event = ReflectionHelper.CreateInstance(message.Assembly, message.Type);

                @event = JsonConvert.DeserializeObject(message.Data, @event.GetType(),
                    SerializerSettings.GetSerializerSettings()) as IIntegrationEvent;

                Task.Run(async () => await _dispatcher.DispatchAsync((IIntegrationEvent) @event)).Wait();

                message.ProcessedAt = DateTime.UtcNow;
            }

            _context.SaveChanges();
        }
    }
}