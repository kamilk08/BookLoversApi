namespace BookLovers.Auth.Domain.Permissions
{
    public static class UserPermissions
    {
        public const string ToManageReview = "ToManageReview";
        public const string ToEditReview = "ToEditReview";
        public const string ToRemoveReview = "ToRemoveReview";
        public const string ToSeeTicket = "ToSeeTicket";

        public static PermissionGroup Group => PermissionGroup.User;
    }
}