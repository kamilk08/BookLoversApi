using System;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Services;
using BookLovers.Filters;
using BookLovers.Publication.Infrastructure.Queries.Quotes;
using BookLovers.Publication.Infrastructure.Root;

namespace BookLovers.PermissionHandlers.Publications
{
    public class ArchiveQuotePermissionHandler : IPermissionHandler
    {
        private const string RequestParameterName = "guid";

        private readonly IModule<PublicationModule> _module;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public string PermissionName => "ToArchiveQuote";

        public ArchiveQuotePermissionHandler(
            IModule<PublicationModule> module,
            IHttpContextAccessor httpContextAccessor)
        {
            _module = module;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<bool> HasPermission(HttpActionContext ctx)
        {
            if (!Guid.TryParse(ctx.ActionArguments[RequestParameterName].ToString(), out var result))
                return false;

            var queryResult = await _module.ExecuteQueryAsync<IsQuoteAddedByQuery, bool>(
                new IsQuoteAddedByQuery(result, _httpContextAccessor.UserGuid));

            return _httpContextAccessor.IsLibrarian() || _httpContextAccessor.IsSuperAdmin() ||
                   queryResult.Value;
        }
    }
}