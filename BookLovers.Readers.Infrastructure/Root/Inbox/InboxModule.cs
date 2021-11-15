using Ninject.Modules;

namespace BookLovers.Readers.Infrastructure.Root.Inbox
{
    internal class InboxModule : NinjectModule
    {
        public override void Load()
        {
            this.Bind<InboxMessagesProcessor>().ToSelf();
        }
    }
}