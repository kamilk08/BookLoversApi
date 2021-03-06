using System;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Services;
using BookLovers.Filters;
using BookLovers.Publication.Infrastructure.Queries;
using BookLovers.Publication.Infrastructure.Root;

namespace BookLovers.PermissionHandlers.Publications
{
    public class ArchiveAuthorPermissionHandler : IPermissionHandler
    {
        private const string RequestParameterName = "guid";

        private readonly IModule<PublicationModule> _module;
        private readonly IHttpContextAccessor _contextAccessor;

        public string PermissionName => "ToArchiveAuthor";

        public ArchiveAuthorPermissionHandler(
            IModule<PublicationModule> module,
            IHttpContextAccessor contextAccessor)
        {
            _module = module;
            _contextAccessor = contextAccessor;
        }

        public async Task<bool> HasPermission(HttpActionContext ctx)
        {
            if (!Guid.TryParse(ctx.ActionArguments[RequestParameterName].ToString(), out var result))
                return false;

            var queryResult =
                await _module.ExecuteQueryAsync<IsAuthorAddedByQuery, bool>(
                    new IsAuthorAddedByQuery(_contextAccessor.UserGuid, result));

            return _contextAccessor.IsLibrarian() || _contextAccessor.IsSuperAdmin() || queryResult.Value;
        }
    }
}