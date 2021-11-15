using System;
using System.Collections.Generic;
using System.Linq;
using BookLovers.Base.Infrastructure.Events.IntegrationEvents;
using BookLovers.Base.Infrastructure.Messages;
using BookLovers.Base.Infrastructure.ModuleCommunication;
using BookLovers.Publication.Infrastructure.Persistence;
using Newtonsoft.Json;
using Ninject;

namespace BookLovers.Publication.Infrastructure.Root.Events
{
    internal class GenericProducerNotifier<TIntegrationEvent> :
        IProducerNotification<TIntegrationEvent>,
        IProducerNotification
        where TIntegrationEvent : IIntegrationEvent
    {
        public void NotifyProducer(TIntegrationEvent @event, Dictionary<int, bool> map)
        {
            var context = CompositionRoot.Kernel.Get<PublicationsContext>();

            using (context)
            {
                if (HasMessage(@event, context))
                    return;

                try
                {
                    if (AllMessagesProcessed(map))
                        AddToOutbox(context, @event, DateTime.UtcNow, map);
                    else
                        AddToOutbox(context, @event, null, map);
                }
                catch (Exception ex)
                {
                    AddToOutbox(context, @event, null, map);
                }
            }
        }

        private void AddToOutbox(
            PublicationsContext context,
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

        private bool HasMessage(TIntegrationEvent @event, PublicationsContext context) => context.OutboxMessages
            .AsNoTracking().Any(a => a.Guid == @event.Guid);

        private bool AllMessagesProcessed(Dictionary<int, bool> map) =>
            map.Values.All(p => p);
    }
}