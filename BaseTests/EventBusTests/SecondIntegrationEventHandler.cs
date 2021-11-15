using System.Threading.Tasks;
using BookLovers.Base.Infrastructure.Events.IntegrationEvents;

namespace BaseTests.EventBusTests
{
    public class SecondIntegrationEventHandler :
        IIntegrationEventHandler<FakeIntegrationEvent>,
        IIntegrationEventHandler
    {
        public Task HandleAsync(FakeIntegrationEvent @event)
        {
            return Task.CompletedTask;
        }
    }
}