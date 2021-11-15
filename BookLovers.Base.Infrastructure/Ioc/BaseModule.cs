using BookLovers.Base.Infrastructure.AppCaching;
using Ninject.Modules;

namespace BookLovers.Base.Infrastructure.Ioc
{
    public class BaseModule : NinjectModule
    {
        public override void Load()
        {
            Bind<CacheService>().ToSelf();
        }
    }
}