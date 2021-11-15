using System;
using System.Linq;
using System.Threading.Tasks;
using BookLovers.Base.Infrastructure.Events.IntegrationEvents;
using BookLovers.Base.Infrastructure.Extensions;
using BookLovers.Base.Infrastructure.Messages;
using BookLovers.Base.Infrastructure.Serialization;
using BookLovers.Readers.Infrastructure.Persistence;
using Newtonsoft.Json;

namespace BookLovers.Readers.Infrastructure.Root.Inbox
{
    internal class InboxMessagesProcessor
    {
        private readonly ReadersContext _context;
        private readonly IIntegrationEventDispatcher _dispatcher;

        public InboxMessagesProcessor(ReadersContext context, IIntegrationEventDispatcher dispatcher)
        {
            this._context = context;
            this._dispatcher = dispatcher;
        }

        public void Process()
        {
            var messages = this._context.InboxMessages
                .OrderBy<InBoxMessage, DateTime>(p => p.OccurredOn)
                .Where(p => p.ProcessedAt == null)
                .ToList();

            foreach (var message in messages)
            {
                var @event = ReflectionHelper.CreateInstance(message.Assembly, message.Type);

                @event = JsonConvert.DeserializeObject(
                    message.Data,
                    @event.GetType(),
                    SerializerSettings.GetSerializerSettings());

                Task.Run(
                        async () => await this._dispatcher.DispatchAsync<IIntegrationEvent>((IIntegrationEvent) @event))
                    .Wait();

                message.ProcessedAt = new DateTime?(DateTime.UtcNow);
            }

            this._context.SaveChanges();
        }
    }
}