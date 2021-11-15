namespace BookLovers.Auth.Domain.Permissions
{
    public static class PublicationPermissions
    {
        public const string ToArchiveBook = "ToArchiveBook";
        public const string ToArchiveAuthor = "ToArchiveAuthor";
        public const string ToEditBook = "ToEditBook";
        public const string ToEditAuthor = "ToEditAuthor";
        public const string ToArchiveQuote = "ToArchiveQuote";
        public const string ToEditQuote = "ToEditQuote";

        public static PermissionGroup Group => PermissionGroup.Publication;
    }
}