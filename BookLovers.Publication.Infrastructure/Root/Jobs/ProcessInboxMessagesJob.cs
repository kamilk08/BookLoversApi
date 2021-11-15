using BookLovers.Base.Infrastructure.Messages;
using BookLovers.Publication.Infrastructure.Root.Inbox;

namespace BookLovers.Publication.Infrastructure.Root.Jobs
{
    internal class ProcessInboxMessagesJob : NonConcurrentJob
    {
        public override void Execute()
        {
            InboxMessagesJobInvoker.Invoke();
        }
    }
}