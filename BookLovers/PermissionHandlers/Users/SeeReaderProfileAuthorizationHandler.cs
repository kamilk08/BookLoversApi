using System.Linq;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Services;
using BookLovers.Filters;
using BookLovers.Readers.Domain.ProfileManagers.Services;
using BookLovers.Readers.Infrastructure.Dtos.Readers;
using BookLovers.Readers.Infrastructure.Queries.Readers.Profiles;
using BookLovers.Readers.Infrastructure.Root;
using BookLovers.Shared.Privacy;

namespace BookLovers.PermissionHandlers.Users
{
    public class SeeProfilePermissionHandler : IPermissionHandler
    {
        private readonly IModule<ReadersModule> _module;
        private readonly IHttpContextAccessor _contextAccessor;

        public string PermissionName => "ToSeeProfile";

        public SeeProfilePermissionHandler(
            IModule<ReadersModule> module,
            IHttpContextAccessor contextAccessor)
        {
            _module = module;
            _contextAccessor = contextAccessor;
        }

        public async Task<bool> HasPermission(HttpActionContext ctx)
        {
            dynamic query = ctx.ActionArguments.First().Value;

            if (query == null) return false;

            var readerId = query.ReaderId;

            if (_contextAccessor.IsLibrarian() || _contextAccessor.IsSuperAdmin())
                return true;

            var queryResult =
                await _module.ExecuteQueryAsync<ReaderProfilePrivacyQuery, ProfilePrivacyDto>(
                    new ReaderProfilePrivacyQuery(readerId));

            if (queryResult?.Value == null)
                return false;

            var privacyOptionDto =
                queryResult.Value.PrivacyOptions.Find(p => p.PrivacyTypeId == ProfilePrivacyType.ProfilePrivacy.Value);

            return privacyOptionDto.PrivacyOptionId == PrivacyOption.Public.Value ||
                   privacyOptionDto.PrivacyOptionId == PrivacyOption.OtherReaders.Value;
        }
    }
}