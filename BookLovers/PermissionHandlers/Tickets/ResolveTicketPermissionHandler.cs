using System.Threading.Tasks;
using System.Web.Http.Controllers;
using BookLovers.Auth.Infrastructure.Queries.Users;
using BookLovers.Auth.Infrastructure.Root;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Services;
using BookLovers.Filters;

namespace BookLovers.PermissionHandlers.Tickets
{
    public class ResolveTicketPermissionHandler : IPermissionHandler
    {
        private readonly IModule<AuthModule> _module;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public string PermissionName => "ToResolveTicket";

        public ResolveTicketPermissionHandler(
            IModule<AuthModule> module,
            IHttpContextAccessor httpContextAccessor)
        {
            _module = module;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<bool> HasPermission(HttpActionContext ctx)
        {
            var queryResult = await _module.ExecuteQueryAsync<IsUserLibrarianQuery, bool>(
                new IsUserLibrarianQuery(_httpContextAccessor.UserGuid));

            return queryResult.Value;
        }
    }
}