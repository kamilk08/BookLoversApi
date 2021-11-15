using System;

namespace BookLovers.Base.Infrastructure.Events.IntegrationEvents
{
    public class IntegrationEvent
    {
        public Guid Guid { get; set; }

        public DateTime EnqueuedAt { get; set; }

        public DateTime? ProcessedAt { get; set; }

        public string Type { get; set; }

        public string Assembly { get; set; }

        public string Data { get; set; }
    }
}