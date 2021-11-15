using BookLovers.Base.Infrastructure.Messages;
using BookLovers.Readers.Infrastructure.Root.Inbox;

namespace BookLovers.Readers.Infrastructure.Root.Jobs
{
    internal class ProcessInboxMessagesJob : NonConcurrentJob
    {
        public override void Execute()
        {
            InboxMessagesJobInvoker.Invoke();
        }
    }
}