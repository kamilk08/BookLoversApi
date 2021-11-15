using System;

namespace BookLovers.Auth.Infrastructure.Persistence.ReadModels
{
    public class PasswordResetTokenReadModel
    {
        public int Id { get; set; }

        public Guid Guid { get; set; }

        public Guid UserGuid { get; set; }

        public string Email { get; set; }

        public string Token { get; set; }

        public DateTime? ExpiresAt { get; set; }

        public int Status { get; set; }
    }
}