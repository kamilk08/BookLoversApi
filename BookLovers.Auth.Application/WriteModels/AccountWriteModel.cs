namespace BookLovers.Auth.Application.WriteModels
{
    public class AccountWriteModel
    {
        public AccountSecurityWriteModel AccountSecurity { get; set; }

        public AccountDetailsWriteModel AccountDetails { get; set; }
    }
}