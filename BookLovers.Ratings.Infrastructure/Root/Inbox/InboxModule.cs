using Ninject.Modules;

namespace BookLovers.Ratings.Infrastructure.Root.Inbox
{
    internal class InboxModule : NinjectModule
    {
        public override void Load()
        {
            Bind<InboxMessagesProcessor>().ToSelf();
        }
    }
}