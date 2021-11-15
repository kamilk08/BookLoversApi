using Ninject;

namespace BookLovers.Auth.Infrastructure.Root.Inbox
{
    internal static class InboxMessagesJobInvoker
    {
        internal static void Invoke()
        {
            CompositionRoot.Kernel.Get<InboxMessagesProcessor>().Process();
        }
    }
}