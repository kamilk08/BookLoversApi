using Ninject.Modules;

namespace BookLovers.Ratings.Infrastructure.Root.Outbox
{
    internal class OutboxModule : NinjectModule
    {
        public override void Load()
        {
            Bind<OutboxMessagesProcessor>().ToSelf();
        }
    }
}