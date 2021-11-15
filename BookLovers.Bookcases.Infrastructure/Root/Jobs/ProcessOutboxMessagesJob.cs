using BookLovers.Base.Infrastructure.Messages;
using BookLovers.Bookcases.Infrastructure.Root.Outbox;

namespace BookLovers.Bookcases.Infrastructure.Root.Jobs
{
    public class ProcessOutboxMessagesJob : NonConcurrentJob
    {
        public override void Execute()
        {
            OutboxMessagesJobInvoker.Invoke();
        }
    }
}