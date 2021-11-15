using System.Collections.Generic;

namespace BookLovers.Filters
{
    public class AuthorizationHandlerFactory
    {
        private readonly IDictionary<string, IPermissionHandler> _handlers;

        public AuthorizationHandlerFactory(IDictionary<string, IPermissionHandler> handlers)
        {
            _handlers = handlers;
        }

        public IPermissionHandler GetAuthorizationHandler(string permission)
        {
            return _handlers[permission];
        }
    }
}