using System.Threading.Tasks;
using System.Web.Http.Controllers;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Services;
using BookLovers.Filters;
using BookLovers.Publication.Application.WriteModels.Books;
using BookLovers.Publication.Infrastructure.Queries;
using BookLovers.Publication.Infrastructure.Root;

namespace BookLovers.PermissionHandlers.Publications
{
    public class EditAuthorPermissionHandler : IPermissionHandler
    {
        private const string RequestParameterName = "writeModel";

        private readonly IModule<PublicationModule> _module;
        private readonly IHttpContextAccessor _contextAccessor;

        public string PermissionName => "ToEditAuthor";

        public EditAuthorPermissionHandler(
            IModule<PublicationModule> module,
            IHttpContextAccessor contextAccessor)
        {
            _module = module;
            _contextAccessor = contextAccessor;
        }

        public async Task<bool> HasPermission(HttpActionContext ctx)
        {
            if (!(ctx.ActionArguments[RequestParameterName] is EditAuthorWriteModel actionArgument))
                return false;

            var queryResult = await _module.ExecuteQueryAsync<IsAuthorAddedByQuery, bool>(
                new IsAuthorAddedByQuery(_contextAccessor.UserGuid, actionArgument.AuthorWriteModel.AuthorGuid));

            return _contextAccessor.IsLibrarian() || _contextAccessor.IsSuperAdmin() || queryResult.Value;
        }
    }
}