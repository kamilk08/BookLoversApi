using System;

namespace BookLovers.Readers.Infrastructure.Dtos.Readers
{
    public class ReaderDto
    {
        public int Id { get; set; }

        public Guid Guid { get; set; }

        public int ReaderId { get; set; }

        public byte Status { get; set; }

        public string Email { get; set; }

        public string UserName { get; set; }

        public string Role { get; set; }

        public DateTime JoinedAt { get; set; }
    }
}