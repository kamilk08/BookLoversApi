using System.Linq;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Services;
using BookLovers.Filters;
using BookLovers.Librarians.Infrastructure.Queries;
using BookLovers.Librarians.Infrastructure.Root;

namespace BookLovers.PermissionHandlers.Tickets
{
    public class SeeTicketAuthorizationHandler : IPermissionHandler
    {
        private readonly IModule<LibrarianModule> _module;
        private readonly IHttpContextAccessor _contextAccessor;

        public string PermissionName => "ToSeeTicket";

        public SeeTicketAuthorizationHandler(
            IModule<LibrarianModule> module,
            IHttpContextAccessor contextAccessor)
        {
            _module = module;
            _contextAccessor = contextAccessor;
        }

        public async Task<bool> HasPermission(HttpActionContext ctx)
        {
            var actionParameter = ctx.ActionArguments.First().Value;
            if (actionParameter == null) return false;

            int ticketId = ((dynamic) actionParameter).TicketId;
            if (ticketId == default(int)) return false;

            var queryResult =
                await _module.ExecuteQueryAsync<IsTicketOwnedByUser, bool>(new IsTicketOwnedByUser(
                    ticketId,
                    _contextAccessor.UserGuid));

            return _contextAccessor.IsLibrarian() || _contextAccessor.IsSuperAdmin() || queryResult.Value;
        }
    }
}