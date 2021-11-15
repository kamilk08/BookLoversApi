using Ninject;

namespace BookLovers.Publication.Infrastructure.Root.Inbox
{
    internal static class InboxMessagesJobInvoker
    {
        internal static void Invoke()
        {
            CompositionRoot.Kernel.Get<InboxMessagesProcessor>().Process();
        }
    }
}