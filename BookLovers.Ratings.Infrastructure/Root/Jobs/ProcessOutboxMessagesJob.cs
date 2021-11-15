using BookLovers.Base.Infrastructure.Messages;
using BookLovers.Ratings.Infrastructure.Root.Outbox;

namespace BookLovers.Ratings.Infrastructure.Root.Jobs
{
    internal class ProcessOutboxMessagesJob : NonConcurrentJob
    {
        public override void Execute()
        {
            OutboxMessagesJobInvoker.Invoke();
        }
    }
}