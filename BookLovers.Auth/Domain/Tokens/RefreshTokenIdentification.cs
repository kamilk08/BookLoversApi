using System;
using BookLovers.Base.Domain.ValueObject;

namespace BookLovers.Auth.Domain.Tokens
{
    public class RefreshTokenIdentification : ValueObject<RefreshTokenIdentification>
    {
        public Guid UserGuid { get; private set; }

        public Guid AudienceGuid { get; private set; }

        private RefreshTokenIdentification()
        {
        }

        public RefreshTokenIdentification(Guid userGuid, Guid audienceGuid)
        {
            UserGuid = userGuid;
            AudienceGuid = audienceGuid;
        }

        protected override int GetHashCodeCore()
        {
            var hash = 17;
            hash = (hash * 23) + this.UserGuid.GetHashCode();
            hash = (hash * 23) + this.AudienceGuid.GetHashCode();

            return hash;
        }

        protected override bool EqualsCore(RefreshTokenIdentification obj) =>
            AudienceGuid == obj.AudienceGuid && UserGuid == obj.UserGuid;
    }
}