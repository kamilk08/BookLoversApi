using System;
using System.Threading.Tasks;
using BookLovers.Auth.Domain.Users.Services;
using BookLovers.Base.Infrastructure.Services;

namespace BookLovers.Auth.Infrastructure.Services.Authorization
{
    public class AuthorizationService : IAuthorizeService
    {
        private readonly IUserRepository _userRepository;

        public AuthorizationService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<bool> AuthorizeAsync(Guid readerGuid, string role)
        {
            var reader = await _userRepository.GetAsync(readerGuid);

            var flag = reader != null && !reader.Account.AccountSecurity.IsBlocked
                                      && reader.IsInRole(role);
            return flag;
        }
    }
}