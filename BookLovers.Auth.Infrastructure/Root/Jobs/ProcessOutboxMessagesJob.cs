using BookLovers.Auth.Infrastructure.Root.Outbox;
using BookLovers.Base.Infrastructure.Messages;

namespace BookLovers.Auth.Infrastructure.Root.Jobs
{
    internal class ProcessOutboxMessagesJob : NonConcurrentJob
    {
        public override void Execute()
        {
            OutboxMessagesJobInvoker.Invoke();
        }
    }
}