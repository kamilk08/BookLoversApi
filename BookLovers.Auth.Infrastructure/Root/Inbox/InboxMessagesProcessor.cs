using System;
using System.Linq;
using System.Threading.Tasks;
using BookLovers.Auth.Infrastructure.Persistence;
using BookLovers.Base.Infrastructure.Events.IntegrationEvents;
using BookLovers.Base.Infrastructure.Extensions;
using BookLovers.Base.Infrastructure.Serialization;
using Newtonsoft.Json;

namespace BookLovers.Auth.Infrastructure.Root.Inbox
{
    internal class InboxMessagesProcessor
    {
        private readonly AuthContext _authContext;
        private readonly IIntegrationEventDispatcher _dispatcher;

        public InboxMessagesProcessor(AuthContext authContext, IIntegrationEventDispatcher dispatcher)
        {
            _authContext = authContext;
            _dispatcher = dispatcher;
        }

        internal void Process()
        {
            var source = _authContext.InboxMessages
                .Where(p => p.ProcessedAt == null)
                .OrderBy(p => p.OccurredOn).ToList();

            foreach (var inBoxMessage in source)
            {
                var @event = ReflectionHelper.CreateInstance(inBoxMessage.Assembly, inBoxMessage.Type);
                @event = JsonConvert.DeserializeObject(
                    inBoxMessage.Data,
                    @event.GetType(),
                    SerializerSettings.GetSerializerSettings());

                Task.Run(async () => await _dispatcher.DispatchAsync(@event as IIntegrationEvent))
                    .Wait();

                inBoxMessage.ProcessedAt = DateTime.UtcNow;
            }

            _authContext.SaveChanges();
        }
    }
}