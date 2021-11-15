using Ninject.Modules;

namespace BookLovers.Librarians.Infrastructure.Root.Inbox
{
    internal class InboxModule : NinjectModule
    {
        public override void Load()
        {
            Bind<InboxMessagesProcessor>().ToSelf();
        }
    }
}