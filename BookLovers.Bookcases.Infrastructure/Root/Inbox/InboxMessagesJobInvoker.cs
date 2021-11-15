using Ninject;

namespace BookLovers.Bookcases.Infrastructure.Root.Inbox
{
    public static class InboxMessagesJobInvoker
    {
        internal static void Invoke()
        {
            CompositionRoot.Kernel.Get<InboxMessagesProcessor>().Process();
        }
    }
}