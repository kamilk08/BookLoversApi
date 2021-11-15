using BookLovers.Auth.Domain.Users;

namespace BookLovers.Auth.Domain.Roles
{
    public interface IRoleProvider
    {
        UserRole GetRole(Role role);
    }
}