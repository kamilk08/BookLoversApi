using System;

namespace BookLovers.Readers.Infrastructure.Dtos.Readers
{
    public class ProfileDto
    {
        public int Id { get; set; }

        public Guid Guid { get; set; }

        public int ReaderId { get; set; }

        public string FirstName { get; set; }

        public string SecondName { get; set; }

        public string FullName { get; set; }

        public string Role { get; set; }

        public string Country { get; set; }

        public string City { get; set; }

        public DateTime? BirthDate { get; set; }

        public DateTime JoinedAt { get; set; }

        public byte Sex { get; set; }

        public string SexName { get; set; }

        public string About { get; set; }

        public string WebSite { get; set; }
    }
}