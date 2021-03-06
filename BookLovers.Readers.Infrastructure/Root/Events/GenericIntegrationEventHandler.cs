using System;
using System.Linq;
using System.Threading.Tasks;
using BookLovers.Base.Infrastructure.Events.IntegrationEvents;
using BookLovers.Base.Infrastructure.Messages;
using BookLovers.Readers.Infrastructure.Persistence;
using Newtonsoft.Json;
using Ninject;

namespace BookLovers.Readers.Infrastructure.Root.Events
{
    internal class GenericIntegrationEventHandler<TIntegrationEvent> :
        IIntegrationEventHandler<TIntegrationEvent>,
        IIntegrationEventHandler
        where TIntegrationEvent : IIntegrationEvent
    {
        public async Task HandleAsync(TIntegrationEvent @event)
        {
            var context = CompositionRoot.Kernel.Get<ReadersContext>();

            var integrationEventHandler = CompositionRoot.Kernel.Get<IIntegrationEventHandler<TIntegrationEvent>>();
            using (context)
            {
                if (this.HasMessage(@event, context)) return;

                try
                {
                    await integrationEventHandler.HandleAsync(@event);
                }
                catch (Exception ex)
                {
                    await this.AddToInboxAsync(context, @event, null);

                    return;
                }

                await this.AddToInboxAsync(context, @event, DateTime.UtcNow);
            }
        }

        private async Task AddToInboxAsync(
            ReadersContext context,
            TIntegrationEvent @event,
            DateTime? processedAt)
        {
            context.InboxMessages.Add(new InBoxMessage()
            {
                Guid = @event.Guid,
                Data = JsonConvert.SerializeObject(@event),
                OccurredOn = @event.OccuredOn,
                Type = @event.GetType().FullName,
                ProcessedAt = processedAt,
                Assembly = @event.GetType().Assembly.FullName
            });

            await context.SaveChangesAsync();
        }

        private bool HasMessage(TIntegrationEvent @event, ReadersContext context) =>
            context.InboxMessages.AsNoTracking().Any(a => a.Guid == @event.Guid);
    }
}