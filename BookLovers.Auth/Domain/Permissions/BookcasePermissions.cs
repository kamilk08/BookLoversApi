namespace BookLovers.Auth.Domain.Permissions
{
    public static class BookcasePermissions
    {
        public const string ToManageBookcase = "ToManageBookcase";
        public const string ToSeeBookcase = "ToSeeBookcase";
        public const string ToSeeReaderBookcase = "ToSeeReaderBookcase";

        public static PermissionGroup Group => PermissionGroup.Bookcase;
    }
}