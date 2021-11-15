using System;

namespace BookLovers.Auth.Infrastructure.Persistence.ReadModels
{
    public class RefreshTokenReadModel
    {
        public int Id { get; set; }

        public Guid TokenGuid { get; set; }

        public Guid UserGuid { get; set; }

        public Guid AudienceGuid { get; set; }

        public DateTime IssuedAt { get; set; }

        public DateTime Expires { get; set; }

        public DateTime? RevokedAt { get; set; }

        public string Hash { get; set; }

        public string Salt { get; set; }

        public string ProtectedTicket { get; set; }

        public byte Status { get; set; }
    }
}