using System;

namespace BookLovers.Base.Infrastructure.Commands
{
    public class InternalCommand
    {
        public int Id { get; set; }

        public Guid Guid { get; set; }

        public DateTime OccuredAt { get; set; }

        public DateTime? ProcessedAt { get; set; }

        public string Data { get; set; }

        public string Type { get; set; }

        public string Assembly { get; set; }

        private InternalCommand()
        {
        }

        public InternalCommand(Guid guid, string data, string type, string assembly)
        {
            Guid = guid;
            OccuredAt = DateTime.UtcNow;
            Data = data;
            Type = type;
            Assembly = assembly;
        }
    }
}