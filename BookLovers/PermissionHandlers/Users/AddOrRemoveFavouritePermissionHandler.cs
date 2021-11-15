using System;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Services;
using BookLovers.Filters;
using BookLovers.Readers.Infrastructure.Queries.Readers.Profiles;
using BookLovers.Readers.Infrastructure.Root;

namespace BookLovers.PermissionHandlers.Users
{
    internal class AddOrRemoveFavouritePermissionHandler : IPermissionHandler
    {
        private const string RequestParameterName = "writeModel";
        private readonly IModule<ReadersModule> _module;
        private readonly IHttpContextAccessor _contextAccessor;

        public string PermissionName => "ToAddOrRemoveFavourite";

        public AddOrRemoveFavouritePermissionHandler(
            IModule<ReadersModule> module,
            IHttpContextAccessor contextAccessor)
        {
            _module = module;
            _contextAccessor = contextAccessor;
        }

        public async Task<bool> HasPermission(HttpActionContext ctx)
        {
            var actionArgument = ctx.ActionArguments[RequestParameterName];
            if (actionArgument == null)
                return false;

            dynamic writeModel = ctx.ActionArguments[RequestParameterName];
            if (writeModel.ProfileGuid == Guid.Empty || writeModel.ProfileGuid == null)
                return false;

            Guid.TryParse(writeModel.ProfileGuid.ToString(), out Guid profileGuid);

            var queryResult =
                await _module.ExecuteQueryAsync<DoesProfileBelongToReaderQuery, bool>(
                    new DoesProfileBelongToReaderQuery(_contextAccessor.UserGuid, profileGuid));

            return queryResult.Value;
        }
    }
}