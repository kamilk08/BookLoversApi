using BookLovers.Base.Domain.ValueObject;

namespace BookLovers.Auth.Domain.Audiences
{
    public class AudienceDetails : ValueObject<AudienceDetails>
    {
        public AudienceType AudienceType { get; private set; }

        public long RefreshTokenLifeTime { get; private set; }

        private AudienceDetails()
        {
        }

        public AudienceDetails(AudienceType audienceType, long refreshTokenLifeTime)
        {
            AudienceType = audienceType;
            RefreshTokenLifeTime = refreshTokenLifeTime;
        }

        public AudienceDetails(int audienceTypeId, long refreshTokenLifeTime)
        {
            AudienceType = AudienceTypes.Get(audienceTypeId);
            RefreshTokenLifeTime = refreshTokenLifeTime;
        }

        public AudienceDetails ChangeApplicationType(AudienceType audienceType) =>
            new AudienceDetails(audienceType, RefreshTokenLifeTime);

        public AudienceDetails ChangeTokenLifeTime(long refreshTokenLifeTime) =>
            new AudienceDetails(AudienceType, refreshTokenLifeTime);

        protected override int GetHashCodeCore()
        {
            var hash = 17;

            hash = (hash * 23) + this.AudienceType.GetHashCode();
            hash = (hash * 23) + this.RefreshTokenLifeTime.GetHashCode();

            return hash;
        }

        protected override bool EqualsCore(AudienceDetails obj)
        {
            return AudienceType.Value == obj.AudienceType.Value
                   && RefreshTokenLifeTime == obj.RefreshTokenLifeTime;
        }
    }
}