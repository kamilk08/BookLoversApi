using System;
using BookLovers.Base.Domain.Aggregates;
using BookLovers.Base.Domain.Rules;

namespace BookLovers.Auth.Domain.Audiences
{
    public class Audience : AggregateRoot
    {
        public AudienceSecurity AudienceSecurity { get; private set; }

        public AudienceDetails AudienceDetails { get; private set; }

        public AudienceState AudienceState { get; private set; }

        private Audience()
        {
        }

        public Audience(Guid guid, AudienceSecurity security, AudienceDetails details)
        {
            Guid = guid;
            AudienceSecurity = security;
            AudienceDetails = details;
            AudienceState = AudienceState.Active;
            Status = AggregateStatus.Active.Value;
        }

        public static Audience Create(
            Guid audienceGuid,
            AudienceSecurity security,
            AudienceDetails details)
        {
            return new Audience(audienceGuid, security, details);
        }

        public void ChangeSecretKey(string hash, string salt) => AudienceSecurity = new AudienceSecurity(hash, salt);

        public void ChangeTokenLifeTime(long tokenLifeTime) =>
            AudienceDetails = AudienceDetails.ChangeTokenLifeTime(tokenLifeTime);

        public void ChangeAudienceType(AudienceType audienceType) =>
            AudienceDetails = AudienceDetails.ChangeApplicationType(audienceType);

        public void Block()
        {
            CheckBusinessRules(new AggregateMustBeActive(Status));

            AudienceState = AudienceState.Blocked;
        }

        public void Activate()
        {
            CheckBusinessRules(new AggregateMustBeArchived(Status));

            AudienceState = AudienceState.Active;
        }

        public bool IsAuthenticated(string generatedHash) =>
            AudienceSecurity.Hash == generatedHash && AudienceState.Value == AudienceState.Active.Value;
    }
}