using Ninject.Modules;

namespace BookLovers.Librarians.Infrastructure.Root.Outbox
{
    internal class OutboxModule : NinjectModule
    {
        public override void Load()
        {
            Bind<OutboxMessagesProcessor>().ToSelf();
        }
    }
}