using BookLovers.Base.Infrastructure.Messages;
using BookLovers.Bookcases.Infrastructure.Root.Inbox;

namespace BookLovers.Bookcases.Infrastructure.Root.Jobs
{
    internal class ProcessInBoxMessagesJob : NonConcurrentJob
    {
        public override void Execute()
        {
            InboxMessagesJobInvoker.Invoke();
        }
    }
}