using System.Threading.Tasks;
using System.Web.Http.Controllers;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Services;
using BookLovers.Filters;
using BookLovers.Readers.Application.WriteModels.Profiles;
using BookLovers.Readers.Infrastructure.Queries.Readers.Profiles;
using BookLovers.Readers.Infrastructure.Root;

namespace BookLovers.PermissionHandlers.Publications
{
    internal class EditProfilePermissionHandler : IPermissionHandler
    {
        private const string RequestParameterName = "writeModel";

        private readonly IModule<ReadersModule> _module;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public string PermissionName => "ToEditProfile";

        public EditProfilePermissionHandler(
            IModule<ReadersModule> module,
            IHttpContextAccessor httpContextAccessor)
        {
            _module = module;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<bool> HasPermission(HttpActionContext ctx)
        {
            if (!(ctx.ActionArguments[RequestParameterName] is ProfileWriteModel writeModel))
                return false;

            var queryResult =
                await _module.ExecuteQueryAsync<DoesProfileBelongToReaderQuery, bool>(
                    new DoesProfileBelongToReaderQuery(_httpContextAccessor.UserGuid, writeModel.ProfileGuid));

            return queryResult.Value;
        }
    }
}