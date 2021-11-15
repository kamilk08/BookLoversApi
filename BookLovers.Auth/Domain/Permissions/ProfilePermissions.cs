namespace BookLovers.Auth.Domain.Permissions
{
    public static class ProfilePermissions
    {
        public const string ToEditProfile = "ToEditProfile";
        public const string ToAddOrRemoveFavourite = "ToAddOrRemoveFavourite";
        public const string ToSeeProfile = "ToSeeProfile";
        public const string ToSeeUserStatistics = "ToSeeUserStatistics";

        public static PermissionGroup Group => PermissionGroup.Profile;
    }
}