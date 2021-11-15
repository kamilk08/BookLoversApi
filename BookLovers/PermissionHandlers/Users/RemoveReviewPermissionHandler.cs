using System;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Services;
using BookLovers.Filters;
using BookLovers.Readers.Infrastructure.Queries.Readers.Reviews;
using BookLovers.Readers.Infrastructure.Root;

namespace BookLovers.PermissionHandlers.Users
{
    public class RemoveReviewPermissionHandler : IPermissionHandler
    {
        private const string RequestParameterName = "guid";

        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IModule<ReadersModule> _module;

        public string PermissionName => "ToRemoveReview";

        public RemoveReviewPermissionHandler(
            IHttpContextAccessor contextAccessor,
            IModule<ReadersModule> module)
        {
            _contextAccessor = contextAccessor;
            _module = module;
        }

        public async Task<bool> HasPermission(HttpActionContext ctx)
        {
            if (!Guid.TryParse(
                ctx.ActionArguments[RequestParameterName].ToString(),
                out var result))
                return false;

            var queryResult = await _module.ExecuteQueryAsync<DoesReviewBelongToReaderQuery, bool>(
                new DoesReviewBelongToReaderQuery(result, _contextAccessor.UserGuid));

            return _contextAccessor.IsLibrarian() || _contextAccessor.IsSuperAdmin() || queryResult.Value;
        }
    }
}