using Ninject;

namespace BookLovers.Librarians.Infrastructure.Root.Outbox
{
    internal static class OutboxMessagesJobInvoker
    {
        internal static void Invoke()
        {
            CompositionRoot.Kernel.Get<OutboxMessagesProcessor>().Process();
        }
    }
}