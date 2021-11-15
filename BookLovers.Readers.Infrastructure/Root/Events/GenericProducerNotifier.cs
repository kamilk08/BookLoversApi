using System;
using System.Collections.Generic;
using System.Linq;
using BookLovers.Base.Infrastructure.Events.IntegrationEvents;
using BookLovers.Base.Infrastructure.Messages;
using BookLovers.Base.Infrastructure.ModuleCommunication;
using BookLovers.Readers.Infrastructure.Persistence;
using Newtonsoft.Json;
using Ninject;

namespace BookLovers.Readers.Infrastructure.Root.Events
{
    internal class GenericProducerNotifier<TIntegrationEvent> :
        IProducerNotification<TIntegrationEvent>,
        IProducerNotification
        where TIntegrationEvent : IIntegrationEvent
    {
        public void NotifyProducer(TIntegrationEvent @event, Dictionary<int, bool> map)
        {
            var context = CompositionRoot.Kernel.Get<ReadersContext>();
            using (context)
            {
                if (this.HasMessage(@event, context))
                    return;
                try
                {
                    if (this.AllMessagesProcessed(map))
                        this.AddToOutbox(context, @event, DateTime.UtcNow, map);
                    else
                        this.AddToOutbox(context, @event, null, map);
                }
                catch (Exception ex)
                {
                    this.AddToOutbox(context, @event, null, map);
                }
            }
        }

        private void AddToOutbox(
            ReadersContext context,
            TIntegrationEvent @event,
            DateTime? processedAt,
            Dictionary<int, bool> map)
        {
            var entity = new OutboxMessage()
            {
                Guid = @event.Guid,
                Data = JsonConvert.SerializeObject(@event),
                OccuredAt = @event.OccuredOn,
                ProcessedAt = processedAt,
                Type = @event.GetType().FullName,
                Assembly = @event.GetType().Assembly.FullName,
                Map = JsonConvert.SerializeObject(map)
            };
            context.OutboxMessages.Add(entity);

            context.SaveChanges();
        }

        private bool HasMessage(TIntegrationEvent @event, ReadersContext context) =>
            context.OutboxMessages
                .AsNoTracking().Any(a => a.Guid == @event.Guid);

        private bool AllMessagesProcessed(Dictionary<int, bool> map) =>
            map.Values.All(p => p);
    }
}