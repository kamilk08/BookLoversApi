using BookLovers.Base.Infrastructure.Messages;
using BookLovers.Ratings.Infrastructure.Root.Inbox;

namespace BookLovers.Ratings.Infrastructure.Root.Jobs
{
    internal class ProcessInboxMessagesJob : NonConcurrentJob
    {
        public override void Execute()
        {
            InboxMessagesJobInvoker.Invoke();
        }
    }
}