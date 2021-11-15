using System;

namespace BookLovers.Auth.Infrastructure.Dtos.Registrations
{
    public class RegistrationSummaryDto
    {
        public int Id { get; set; }

        public Guid Guid { get; set; }

        public Guid UserGuid { get; set; }

        public string Email { get; set; }
    }
}