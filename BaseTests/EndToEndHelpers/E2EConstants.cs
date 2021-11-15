namespace BaseTests.EndToEndHelpers
{
    public static class E2EConstants
    {
        public static readonly string SuperAdminEmail = "superadmin@gmail.com";

        public static readonly string DefaultUserEmail = "testemail@gmail.com";
        public static readonly string DefaultUserNickName = "username";
        public static readonly string SecondUserEmail = "testemail2@gmail.com";
        public static readonly string SecondUserNickName = "testemail2";

        public static readonly string UserToBlockEmail = "userToBlock@gmail.com";
        public static readonly string UserToBlockNickName = "userToBlock";
        public static readonly string DefaultPassword = "Babcia123!";

        public static readonly string FollowerEmail = "follower@gmail.com";
        public static readonly string FollowerNickName = "follower";

        public static readonly string ClientSecret = "DUPA";

        public static readonly string AudienceGuid = "00f80a32-0205-4aff-94d9-46635d8c431c";

        public static readonly string AuthConnectionString =
            @"Server=localhost\SQLEXPRESS;Database=AuthorizationContext;Trusted_Connection=True;";

        public static readonly string BookcaseConnectionString =
            @"Server=localhost\SQLEXPRESS;Database=BookcaseContext;Trusted_Connection=True;";

        public static readonly string BookcaseStoreConnectionString =
            @"Server=localhost\SQLEXPRESS;Database=BookcaseStoreContext;Trusted_Connection=True;";

        public static readonly string PublicationsConnectionString =
            @"Server=localhost\SQLEXPRESS;Database=PublicationsContext;Trusted_Connection=True;";

        public static readonly string PublicationsStoreConnectionString =
            @"Server=localhost\SQLEXPRESS;Database=PublicationsStoreContext;Trusted_Connection=True;";

        public static readonly string LibrariansConnectionString =
            @"Server=localhost\SQLEXPRESS;Database=LibrariansContext;Trusted_Connection=True;";

        public static readonly string RatingsConnectionString =
            @"Server=localhost\SQLEXPRESS;Database=RatingsContext;Trusted_Connection=True;";

        public static readonly string ReadersConnectionString =
            @"Server=localhost\SQLEXPRESS;Database=ReadersContext;Trusted_Connection=True;";

        public static readonly string ReadersStoreConnectionString =
            @"Server=localhost\SQLEXPRESS;Database=ReadersStoreContext;Trusted_Connection=True;";
    }
}