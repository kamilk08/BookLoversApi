using BookLovers.Base.Domain.Entity;

namespace BookLovers.Auth.Domain.Users
{
    public class Account : IEntityObject
    {
        public int Id { get; private set; }

        public Email Email { get; internal set; }

        public AccountSecurity AccountSecurity { get; internal set; }

        public AccountDetails AccountDetails { get; private set; }

        public AccountConfirmation AccountConfirmation { get; internal set; }

        private Account()
        {
        }

        public Account(Email email, AccountSecurity accountSecurity, AccountDetails accountDetails)
        {
            Email = email;
            AccountSecurity = accountSecurity;
            AccountDetails = accountDetails;
            AccountConfirmation = AccountConfirmation.NotConfirmed();
        }

        internal void BlockAccount()
        {
            AccountSecurity = AccountSecurity.BlockAccount();
            AccountDetails = new AccountDetails(AccountDetails.AccountCreateDate, true);
        }

        public bool IsAccountActive() => !AccountSecurity.IsBlocked && AccountConfirmation.IsConfirmed;
    }
}