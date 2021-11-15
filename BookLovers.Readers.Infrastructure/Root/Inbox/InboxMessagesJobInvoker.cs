using Ninject;

namespace BookLovers.Readers.Infrastructure.Root.Inbox
{
    internal static class InboxMessagesJobInvoker
    {
        internal static void Invoke()
        {
            CompositionRoot.Kernel.Get<InboxMessagesProcessor>().Process();
        }
    }
}