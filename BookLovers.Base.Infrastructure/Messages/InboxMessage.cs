using System;

namespace BookLovers.Base.Infrastructure.Messages
{
    public class InBoxMessage
    {
        public Guid Guid { get; set; }

        public DateTime OccurredOn { get; set; }

        public DateTime? ProcessedAt { get; set; }

        public string Type { get; set; }

        public string Data { get; set; }

        public string Assembly { get; set; }
    }
}