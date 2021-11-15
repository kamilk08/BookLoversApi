using BookLovers.Base.Infrastructure.Messages;
using BookLovers.Publication.Infrastructure.Root.Outbox;

namespace BookLovers.Publication.Infrastructure.Root.Jobs
{
    internal class ProcessOutboxMessagesJob : NonConcurrentJob
    {
        public override void Execute()
        {
            OutboxMessagesJobInvoker.Invoke();
        }
    }
}