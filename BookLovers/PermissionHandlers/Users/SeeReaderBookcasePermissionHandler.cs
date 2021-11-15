using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using BookLovers.Auth.Infrastructure.Dtos.Tokens;
using BookLovers.Auth.Infrastructure.Queries.Tokens;
using BookLovers.Auth.Infrastructure.Root;
using BookLovers.Auth.Infrastructure.Services;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Services;
using BookLovers.Bookcases.Infrastructure.Dtos;
using BookLovers.Bookcases.Infrastructure.Queries.Bookcases;
using BookLovers.Bookcases.Infrastructure.Root;
using BookLovers.Filters;
using BookLovers.Readers.Infrastructure.Queries.Readers.Follows;
using BookLovers.Readers.Infrastructure.Root;
using BookLovers.Shared.Privacy;

namespace BookLovers.PermissionHandlers.Users
{
    public class SeeReaderBookcasePermissionHandler : IPermissionHandler
    {
        private readonly IModule<BookcaseModule> _bookcaseModule;
        private readonly IModule<AuthModule> _authModule;
        private readonly IModule<ReadersModule> _readersModule;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IAppManager _appManager;

        public string PermissionName => "ToSeeReaderBookcase";

        public SeeReaderBookcasePermissionHandler(
            IModule<BookcaseModule> bookcaseModule,
            IModule<AuthModule> authModule,
            IModule<ReadersModule> readersModule,
            IHttpContextAccessor httpContextAccessor,
            IAppManager appManager)
        {
            _bookcaseModule = bookcaseModule;
            _authModule = authModule;
            _readersModule = readersModule;
            _httpContextAccessor = httpContextAccessor;
            _appManager = appManager;
        }

        public async Task<bool> HasPermission(HttpActionContext ctx)
        {
            dynamic dto = ctx.ActionArguments.First().Value;

            if (dto == null) return false;

            var bookcaseQueryResult =
                await _bookcaseModule.ExecuteQueryAsync<BookcaseByReaderIdQuery, BookcaseDto>(
                    new BookcaseByReaderIdQuery(dto.ReaderId));

            if (bookcaseQueryResult.Value == null)
                return true;

            var protectedToken = string.Empty;

            if (ctx.Request.Headers.Contains("Authorization"))
                protectedToken = ctx.Request.Headers.Authorization.Parameter;

            var issuer = _appManager.GetConfigValue(JwtSettings.Issuer);

            var queryResult =
                await _authModule.ExecuteQueryAsync<GetClaimsIdentityFromProtectedTokenQuery, ClaimsIdentityDto>(
                    new GetClaimsIdentityFromProtectedTokenQuery(protectedToken, issuer));

            if (queryResult?.Value.ClaimsIdentity == null)
            {
                if (bookcaseQueryResult.Value.BookcaseOptions.Privacy == PrivacyOption.Public.Value)
                    return true;
            }
            else
            {
                var claim = GetUserGuidFromClaims(queryResult.Value);

                var parsedSuccessfully = Guid.TryParse(claim.Value, out var result);

                if ((parsedSuccessfully && result == bookcaseQueryResult.Value.ReaderGuid) ||
                    bookcaseQueryResult.Value.BookcaseOptions.Privacy == PrivacyOption.Public.Value ||
                    bookcaseQueryResult.Value.BookcaseOptions.Privacy == PrivacyOption.OtherReaders.Value)
                    return true;

                if (bookcaseQueryResult.Value.BookcaseOptions.Privacy == PrivacyOption.Private.Value)
                {
                    var isFollowingQueryResult = await _readersModule.ExecuteQueryAsync<IsFollowingQuery, bool>(
                        new IsFollowingQuery(bookcaseQueryResult.Value.ReaderId));

                    return isFollowingQueryResult.Value;
                }
            }

            return false;
        }

        private Claim GetUserGuidFromClaims(ClaimsIdentityDto queryResult)
        {
            return queryResult.ClaimsIdentity.Claims.First();
        }
    }
}