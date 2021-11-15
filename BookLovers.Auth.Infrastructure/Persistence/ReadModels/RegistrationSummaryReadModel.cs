using System;

namespace BookLovers.Auth.Infrastructure.Persistence.ReadModels
{
    public class RegistrationSummaryReadModel
    {
        public int Id { get; set; }

        public Guid Guid { get; set; }

        public Guid UserGuid { get; set; }

        public string Email { get; set; }

        public string Token { get; set; }

        public DateTime CreateAt { get; set; }

        public DateTime? CompletedAt { get; set; }

        public DateTime ExpiredAt { get; set; }

        public int Status { get; set; }
    }
}