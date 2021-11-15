using System.Threading.Tasks;
using System.Web.Http.Controllers;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Services;
using BookLovers.Bookcases.Infrastructure.Dtos;
using BookLovers.Bookcases.Infrastructure.Queries.Bookcases;
using BookLovers.Bookcases.Infrastructure.Root;
using BookLovers.Filters;

namespace BookLovers.PermissionHandlers.Users
{
    public class ManageBookcasePermissionHandler : IPermissionHandler
    {
        private const string RequestParameterName = "writeModel";
        private readonly IModule<BookcaseModule> _module;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public string PermissionName => "ToManageBookcase";

        public ManageBookcasePermissionHandler(
            IModule<BookcaseModule> module,
            IHttpContextAccessor httpContextAccessor)
        {
            _module = module;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<bool> HasPermission(HttpActionContext ctx)
        {
            var actionArgument = ctx.ActionArguments[RequestParameterName];
            if (actionArgument == null) return false;

            dynamic writeModel = ctx.ActionArguments[RequestParameterName];

            var queryResult =
                await _module.ExecuteQueryAsync<BookcaseByReaderGuidQuery, BookcaseDto>(
                    new BookcaseByReaderGuidQuery(_httpContextAccessor.UserGuid));

            return writeModel.BookcaseGuid == queryResult.Value.Guid;
        }
    }
}