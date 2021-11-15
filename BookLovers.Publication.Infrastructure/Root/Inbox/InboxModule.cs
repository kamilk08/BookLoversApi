using Ninject.Modules;

namespace BookLovers.Publication.Infrastructure.Root.Inbox
{
    internal class InboxModule : NinjectModule
    {
        public override void Load()
        {
            Bind<InboxMessagesProcessor>().ToSelf();
        }
    }
}