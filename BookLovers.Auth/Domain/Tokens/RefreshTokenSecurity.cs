using BookLovers.Base.Domain.ValueObject;

namespace BookLovers.Auth.Domain.Tokens
{
    public class RefreshTokenSecurity : ValueObject<RefreshTokenSecurity>
    {
        public string Hash { get; private set; }

        public string Salt { get; private set; }

        private RefreshTokenSecurity()
        {
        }

        public RefreshTokenSecurity(string hash, string salt)
        {
            Hash = hash;
            Salt = salt;
        }

        protected override int GetHashCodeCore()
        {
            var hash = 17;

            hash = (hash * 23) + this.Hash.GetHashCode();
            hash = (hash * 23) + this.Salt.GetHashCode();

            return hash;
        }

        protected override bool EqualsCore(RefreshTokenSecurity obj) =>
            Hash == obj.Hash && Salt == obj.Salt;
    }
}