using BookLovers.Base.Infrastructure.Messages;
using BookLovers.Readers.Infrastructure.Root.Outbox;

namespace BookLovers.Readers.Infrastructure.Root.Jobs
{
    internal class ProcessOutboxMessagesJob : NonConcurrentJob
    {
        public override void Execute()
        {
            OutboxMessagesJobInvoker.Invoke();
        }
    }
}