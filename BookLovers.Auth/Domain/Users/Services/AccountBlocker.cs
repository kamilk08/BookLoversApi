using BookLovers.Base.Domain.Rules;
using BookLovers.Base.Domain.Services;
using BookLovers.Base.Infrastructure.Services;

namespace BookLovers.Auth.Domain.Users.Services
{
    public class AccountBlocker : IDomainService<User>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AccountBlocker(IHttpContextAccessor httpContextAccessor) => _httpContextAccessor = httpContextAccessor;

        public void BlockUser(User user)
        {
            if (user == null)
                throw new BusinessRuleNotMetException("User either does not exist or has been suspended.");

            if (!user.IsInRole(Role.Librarian.Name))
                user.BlockAccount();
            else
            {
                if (!_httpContextAccessor.IsSuperAdmin())
                    throw new BusinessRuleNotMetException("Only super admin can block other librarian.");

                user.BlockAccount();
            }
        }
    }
}