namespace BookLovers.Auth.Domain.Permissions
{
    public static class LibrarianPermissions
    {
        public const string ToResolveTicket = "ToResolveTicket";

        public static PermissionGroup Group => PermissionGroup.Librarian;
    }
}