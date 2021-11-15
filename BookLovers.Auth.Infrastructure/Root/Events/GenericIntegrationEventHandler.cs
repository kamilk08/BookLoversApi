using System;
using System.Linq;
using System.Threading.Tasks;
using BookLovers.Auth.Infrastructure.Persistence;
using BookLovers.Base.Infrastructure.Events.IntegrationEvents;
using BookLovers.Base.Infrastructure.Messages;
using Newtonsoft.Json;
using Ninject;

namespace BookLovers.Auth.Infrastructure.Root.Events
{
    internal class GenericIntegrationEventHandler<T> :
        IIntegrationEventHandler<T>,
        IIntegrationEventHandler
        where T : IIntegrationEvent
    {
        public async Task HandleAsync(T @event)
        {
            var authContext = CompositionRoot.Kernel.Get<AuthContext>();
            var integrationHandler = CompositionRoot.Kernel.Get<IIntegrationEventHandler<T>>();
            using (authContext)
            {
                if (HasMessage(@event, authContext))
                    return;

                try
                {
                    await integrationHandler.HandleAsync(@event);
                }
                catch (Exception ex)
                {
                    await AddToInboxAsync(authContext, @event, null);
                }

                await AddToInboxAsync(authContext, @event, DateTime.UtcNow);
            }
        }

        private async Task AddToInboxAsync(
            AuthContext authContext,
            T @event,
            DateTime? processedAt)
        {
            var message = new InBoxMessage()
            {
                Guid = @event.Guid,
                Data = JsonConvert.SerializeObject(@event),
                OccurredOn = @event.OccuredOn,
                Type = @event.GetType().FullName,
                ProcessedAt = processedAt,
                Assembly = @event.GetType().Assembly.FullName
            };

            authContext.InboxMessages.Add(message);
            await authContext.SaveChangesAsync();
        }

        private bool HasMessage(T @event, AuthContext context)
        {
            return context.InboxMessages.AsNoTracking()
                .Any(a => a.Guid == @event.Guid);
        }
    }
}