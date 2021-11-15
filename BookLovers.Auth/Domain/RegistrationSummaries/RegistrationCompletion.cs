using System;
using BookLovers.Base.Domain.ValueObject;

namespace BookLovers.Auth.Domain.RegistrationSummaries
{
    public class RegistrationCompletion : ValueObject<RegistrationCompletion>
    {
        public DateTime CreatedAt { get; private set; }

        public DateTime? CompletedAt { get; private set; }

        public DateTime ExpiresAt { get; private set; }

        private RegistrationCompletion()
        {
        }

        private RegistrationCompletion(DateTime createdAt, DateTime expiresAt)
        {
            CreatedAt = createdAt;
            CompletedAt = null;
            ExpiresAt = expiresAt;
        }

        private RegistrationCompletion(DateTime createdAt, DateTime expiresAt, DateTime completedAt)
        {
            CreatedAt = createdAt;
            CompletedAt = completedAt;
            ExpiresAt = expiresAt;
        }

        public static RegistrationCompletion NotCompleted() =>
            new RegistrationCompletion(DateTime.UtcNow, DateTime.UtcNow.AddDays(1));

        public RegistrationCompletion Completed(DateTime completedAt) =>
            new RegistrationCompletion(CreatedAt, ExpiresAt, completedAt);

        protected override int GetHashCodeCore()
        {
            var hash = 17;

            hash = (hash * 23) + this.CompletedAt.GetHashCode();
            hash = (hash * 23) + this.ExpiresAt.GetHashCode();
            hash = (hash * 23) + this.CreatedAt.GetHashCode();

            return hash;
        }

        protected override bool EqualsCore(RegistrationCompletion obj)
        {
            return this.CompletedAt == obj.CompletedAt && this.ExpiresAt == obj.ExpiresAt &&
                   this.CreatedAt == obj.CreatedAt;
        }
    }
}