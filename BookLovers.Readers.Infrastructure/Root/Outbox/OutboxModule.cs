using Ninject.Modules;

namespace BookLovers.Readers.Infrastructure.Root.Outbox
{
    internal class OutboxModule : NinjectModule
    {
        public override void Load()
        {
            this.Bind<OutboxMessagesProcessor>().ToSelf();
        }
    }
}