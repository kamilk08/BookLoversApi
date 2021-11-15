using BookLovers.Base.Infrastructure.Messages;
using BookLovers.Librarians.Infrastructure.Root.Inbox;

namespace BookLovers.Librarians.Infrastructure.Root.Jobs
{
    internal class ProcessInboxMessagesJob : NonConcurrentJob
    {
        public override void Execute()
        {
            InboxMessagesJobInvoker.Invoke();
        }
    }
}