using Ninject.Modules;

namespace BookLovers.Auth.Infrastructure.Root.Outbox
{
    internal class OutboxModule : NinjectModule
    {
        public override void Load()
        {
            Bind<OutboxMessagesProcessor>().ToSelf();
        }
    }
}