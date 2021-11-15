using BookLovers.Auth.Infrastructure.Root.Inbox;
using BookLovers.Base.Infrastructure.Messages;

namespace BookLovers.Auth.Infrastructure.Root.Jobs
{
    public class ProcessInboxMessagesJob : NonConcurrentJob
    {
        public override void Execute()
        {
            InboxMessagesJobInvoker.Invoke();
        }
    }
}