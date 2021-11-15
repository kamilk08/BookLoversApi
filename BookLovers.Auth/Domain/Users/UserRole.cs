using BookLovers.Base.Domain.ValueObject;

namespace BookLovers.Auth.Domain.Users
{
    public class UserRole : ValueObject<UserRole>
    {
        public Role Role { get; private set; }

        private UserRole()
        {
        }

        public UserRole(Role role) => Role = role;

        protected override int GetHashCodeCore() => (17 * 23) + Role.GetHashCode();

        protected override bool EqualsCore(UserRole obj) => Role == obj.Role;
    }
}