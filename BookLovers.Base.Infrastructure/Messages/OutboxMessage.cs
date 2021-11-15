using System;

namespace BookLovers.Base.Infrastructure.Messages
{
    public class OutboxMessage
    {
        public Guid Guid { get; set; }

        public DateTime OccuredAt { get; set; }

        public DateTime? ProcessedAt { get; set; }

        public string Type { get; set; }

        public string Assembly { get; set; }

        public string Data { get; set; }

        public string Map { get; set; }
    }
}