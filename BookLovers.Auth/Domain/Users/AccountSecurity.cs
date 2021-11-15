using BookLovers.Base.Domain.ValueObject;

namespace BookLovers.Auth.Domain.Users
{
    public class AccountSecurity : ValueObject<AccountSecurity>
    {
        public string Hash { get; private set; }

        public string Salt { get; private set; }

        public bool IsBlocked { get; private set; }

        private AccountSecurity()
        {
        }

        public AccountSecurity(string hash, string salt, bool isBlocked)
        {
            Hash = hash;
            Salt = salt;
            IsBlocked = isBlocked;
        }

        public AccountSecurity BlockAccount() => new AccountSecurity(Hash, Salt, true);

        public AccountSecurity ChangePassword(string salt, string hash) => new AccountSecurity(hash, salt, IsBlocked);

        protected override int GetHashCodeCore()
        {
            var hash = 17;
            hash = (hash * 23) + this.IsBlocked.GetHashCode();
            hash = (hash * 23) + this.Hash.GetHashCode();
            hash = (hash * 23) + this.Salt.GetHashCode();

            return hash;
        }

        protected override bool EqualsCore(AccountSecurity obj) =>
            IsBlocked == obj.IsBlocked && Hash == obj.Hash && Salt == obj.Salt;
    }
}