using Ninject.Modules;

namespace BookLovers.Bookcases.Infrastructure.Root.Outbox
{
    internal class OutboxModule : NinjectModule
    {
        public override void Load()
        {
            Bind<OutboxMessagesProcessor>().ToSelf();
        }
    }
}