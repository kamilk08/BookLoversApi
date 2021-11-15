using System.Threading.Tasks;
using System.Web.Http.Controllers;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Services;
using BookLovers.Filters;
using BookLovers.Readers.Application.WriteModels.Reviews;
using BookLovers.Readers.Infrastructure.Queries.Readers.Reviews;
using BookLovers.Readers.Infrastructure.Root;

namespace BookLovers.PermissionHandlers.Publications
{
    public class EditReviewPermissionHandler : IPermissionHandler
    {
        private const string RequestParameterName = "writeModel";

        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IModule<ReadersModule> _module;

        public string PermissionName => "ToEditReview";

        public EditReviewPermissionHandler(
            IHttpContextAccessor contextAccessor,
            IModule<ReadersModule> module)
        {
            _contextAccessor = contextAccessor;
            _module = module;
        }

        public async Task<bool> HasPermission(HttpActionContext ctx)
        {
            if (!(ctx.ActionArguments[RequestParameterName] is ReviewWriteModel actionArgument))
                return false;

            var queryResult = await _module.ExecuteQueryAsync<DoesReviewBelongToReaderQuery, bool>(
                new DoesReviewBelongToReaderQuery(actionArgument.ReviewGuid, _contextAccessor.UserGuid));

            return _contextAccessor.IsLibrarian() || _contextAccessor.IsSuperAdmin() || queryResult.Value;
        }
    }
}