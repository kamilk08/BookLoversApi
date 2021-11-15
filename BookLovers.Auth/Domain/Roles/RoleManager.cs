using System.Linq;
using BookLovers.Auth.Domain.Users;
using BookLovers.Base.Domain.Rules;
using BookLovers.Base.Domain.Services;

namespace BookLovers.Auth.Domain.Roles
{
    public class RoleManager : IDomainService<User>
    {
        private static readonly RoleRulesCollection Collection = new RoleRulesCollection();

        public static void Promote(User user, Role role)
        {
            var userRole = IsUserSuitableFor(user, role)
                ? new UserRole(role)
                : throw new BusinessRuleNotMetException("User cannot be promoted to selected role.");

            user.UserRoles.Add(userRole);
        }

        public static void Degrade(User user, Role role)
        {
            if (!IsUserSuitableFor(user, role))
                throw new BusinessRuleNotMetException("User must have at least one role.");

            var userRole = user.UserRoles.SingleOrDefault(p => p.Role.Value == role.Value);

            user.UserRoles.Remove(userRole);
        }

        private static bool IsUserSuitableFor(User user, Role role) =>
            Collection.GetRule(role)(user).IsFulfilled();
    }
}