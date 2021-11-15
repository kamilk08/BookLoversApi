using BookLovers.Base.Domain.Extensions;

namespace BookLovers.Auth.Domain.Users
{
    public class Role : Enumeration
    {
        public static readonly Role Reader = new Role(1, nameof(Reader));
        public static readonly Role Librarian = new Role(2, nameof(Librarian));
        public static readonly Role SuperAdmin = new Role(3, nameof(SuperAdmin));

        protected Role()
        {
        }

        public Role(int value, string name)
            : base(value, name)
        {
        }
    }
}