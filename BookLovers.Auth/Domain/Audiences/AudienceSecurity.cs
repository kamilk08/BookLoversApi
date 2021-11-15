using BookLovers.Base.Domain.ValueObject;

namespace BookLovers.Auth.Domain.Audiences
{
    public class AudienceSecurity : ValueObject<AudienceSecurity>
    {
        public string Hash { get; private set; }

        public string Salt { get; private set; }

        private AudienceSecurity()
        {
        }

        public AudienceSecurity(string hash, string salt)
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

        protected override bool EqualsCore(AudienceSecurity obj)
        {
            return Hash == obj.Hash && Salt == obj.Salt;
        }
    }
}