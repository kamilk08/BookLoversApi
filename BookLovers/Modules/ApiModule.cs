using System.Collections.Generic;
using BookLovers.Filters;
using Ninject.Extensions.Conventions;
using Ninject.Modules;

namespace BookLovers.Modules
{
    public class ApiModule : NinjectModule
    {
        public override void Load()
        {
            this.Bind(x =>
                x.FromAssemblyContaining(typeof(IPermissionHandler))
                    .IncludingNonPublicTypes().SelectAllClasses()
                    .InheritedFrom(typeof(IPermissionHandler))
                    .BindAllInterfaces());

            Bind<IDictionary<string, IPermissionHandler>>().ToProvider<AuthorizationHandlerProvider>();

            Bind<AuthorizationHandlerFactory>().ToSelf();
        }
    }
}