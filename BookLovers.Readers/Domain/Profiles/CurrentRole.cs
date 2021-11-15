using BookLovers.Base.Domain.ValueObject;

namespace BookLovers.Readers.Domain.Profiles
{
    public class CurrentRole : ValueObject<CurrentRole>
    {
        public string RoleName { get; }

        private CurrentRole()
        {
        }

        internal CurrentRole(string roleName)
        {
            RoleName = roleName;
        }

        public static CurrentRole User()
        {
            return new CurrentRole(nameof(User));
        }

        public static CurrentRole Librarian()
        {
            return new CurrentRole(nameof(Librarian));
        }

        public static CurrentRole SuperAdmin()
        {
            return new CurrentRole(nameof(SuperAdmin));
        }

        protected override int GetHashCodeCore()
        {
            return (17 * 23) + RoleName.GetHashCode();
        }

        protected override bool EqualsCore(CurrentRole obj)
        {
            return RoleName == obj.RoleName;
        }
    }
}