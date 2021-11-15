using BookLovers.Base.Domain.Extensions;

namespace BookLovers.Auth.Domain.Permissions
{
    public class PermissionGroup : Enumeration
    {
        public static readonly PermissionGroup User = new PermissionGroup(1, nameof(User));
        public static readonly PermissionGroup Bookcase = new PermissionGroup(2, nameof(Bookcase));
        public static readonly PermissionGroup Publication = new PermissionGroup(3, nameof(Publication));
        public static readonly PermissionGroup Librarian = new PermissionGroup(4, nameof(Librarian));
        public static readonly PermissionGroup Profile = new PermissionGroup(5, nameof(Profile));

        protected PermissionGroup()
        {
        }

        protected PermissionGroup(byte value, string name)
            : base(value, name)
        {
        }
    }
}