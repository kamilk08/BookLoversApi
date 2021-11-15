using System;
using BookLovers.Auth.Domain.RegistrationSummaries.BusinessRules;
using BookLovers.Auth.Events;
using BookLovers.Base.Domain.Aggregates;

namespace BookLovers.Auth.Domain.RegistrationSummaries
{
    public class RegistrationSummary : AggregateRoot
    {
        public RegistrationIdentification Identification { get; private set; }

        public RegistrationCompletion Completion { get; private set; }

        public string Token { get; private set; }

        private RegistrationSummary()
        {
        }

        private RegistrationSummary(Guid guid, RegistrationIdentification identification, string token)
        {
            Guid = guid;
            Identification = identification;
            Completion = RegistrationCompletion.NotCompleted();
            Token = token;
            Status = AggregateStatus.Active.Value;
        }

        public static RegistrationSummary Create(RegistrationIdentification identification, string token)
        {
            return new RegistrationSummary(Guid.NewGuid(), identification, token);
        }

        public void Complete(string token)
        {
            CheckBusinessRules(new RegistrationCompletionRules(this, token));

            var now = DateTime.UtcNow;

            Completion = Completion.Completed(now);

            Events.Add(new RegistrationSummaryCompleted(Guid, Identification.UserGuid, now));
        }

        public void EndWithoutCompletion()
        {
            CheckBusinessRules(new FinishRegistrationWithoutCompletionRules(this));

            Events.Add(new RegistrationSummaryNotCompleted(Guid, Identification.UserGuid));
        }

        public bool AreTokensEqual(string protectedToken) => protectedToken == Token;

        public bool IsExpired() => DateTime.UtcNow > Completion.ExpiresAt;
    }
}