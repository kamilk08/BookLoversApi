using System;

namespace BookLovers.Auth.Infrastructure.Persistence.ReadModels
{
    public class AudienceReadModel
    {
        public int Id { get; set; }

        public Guid AudienceGuid { get; set; }

        public string Hash { get; set; }

        public string Salt { get; set; }

        public string AudienceName { get; set; }

        public int AudienceType { get; set; }

        public long RefreshTokenLifeTime { get; set; }

        public int AudienceState { get; set; }

        public string AudienceStateName { get; set; }

        public int Status { get; set; }
    }
}