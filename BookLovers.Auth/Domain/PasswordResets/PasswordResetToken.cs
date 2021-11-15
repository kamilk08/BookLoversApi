using System;
using BookLovers.Auth.Domain.Users;
using BookLovers.Auth.Events;
using BookLovers.Base.Domain.Aggregates;

namespace BookLovers.Auth.Domain.PasswordResets
{
    public class PasswordResetToken : AggregateRoot
    {
        internal static TimeSpan DefaultResetTimeSpan = TimeSpan.FromMinutes(5);

        public Guid UserGuid { get; private set; }

        public string Email { get; private set; }

        public string Token { get; private set; }

        public DateTime? ExpiresAt { get; private set; }

        private PasswordResetToken()
        {
        }

        public PasswordResetToken(Guid userGuid, string email)
        {
            Guid = Guid.NewGuid();
            UserGuid = userGuid;
            Email = email;
            Status = AggregateStatus.Active.Value;
        }

        public PasswordResetToken(Guid userGuid, string email, DateTime expiresAt, string token)
        {
            UserGuid = userGuid;
            Email = email;
            ExpiresAt = expiresAt;
            Token = token;
            Status = AggregateStatus.Active.Value;
        }

        public void Generate(User user)
        {
            CheckBusinessRules(new GeneratePasswordResetTokenRules(user, DefaultResetTimeSpan));

            this.Token = Guid.NewGuid().ToString();
            this.ExpiresAt = DateTime.UtcNow.Add(DefaultResetTimeSpan);

            Events.Add(new ResetPasswordTokenGenerated(Guid, Token));
        }

        public void Generate(User user, TimeSpan resetTime)
        {
            CheckBusinessRules(new GeneratePasswordResetTokenRules(user, resetTime));

            this.Token = Guid.NewGuid().ToString();
            this.ExpiresAt = DateTime.UtcNow.Add(resetTime);

            Events.Add(new ResetPasswordTokenGenerated(Guid, Token));
        }

        public bool IsExpired()
        {
            return this.ExpiresAt.HasValue
                   && DateTime.UtcNow > this.ExpiresAt.GetValueOrDefault();
        }
    }
}