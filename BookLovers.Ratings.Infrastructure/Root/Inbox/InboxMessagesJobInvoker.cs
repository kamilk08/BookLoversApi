using Ninject;

namespace BookLovers.Ratings.Infrastructure.Root.Inbox
{
    internal static class InboxMessagesJobInvoker
    {
        public static void Invoke()
        {
            CompositionRoot.Kernel.Get<InboxMessagesProcessor>().Process();
        }
    }
}