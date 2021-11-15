using BookLovers.Base.Infrastructure.Messages;
using BookLovers.Librarians.Infrastructure.Root.Outbox;

namespace BookLovers.Librarians.Infrastructure.Root.Jobs
{
    internal class ProcessOutboxMessagesJob : NonConcurrentJob
    {
        public override void Execute()
        {
            OutboxMessagesJobInvoker.Invoke();
        }
    }
}