using System;
using System.Collections.Generic;
using System.Linq;
using BookLovers.Base.Infrastructure.Ioc;
using Ninject;
using Ninject.Activation;

namespace BookLovers.Filters
{
    public class AuthorizationHandlerProvider : BaseProvider<IDictionary<string, IPermissionHandler>>
    {
        public override Type Type => typeof(IDictionary<string, IPermissionHandler>);

        protected override IDictionary<string, IPermissionHandler> CreateInstance(
            IContext context)
        {
            return context.Kernel.GetAll<IPermissionHandler>().ToDictionary(k => k.PermissionName, v => v);
        }
    }
}