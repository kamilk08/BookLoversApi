using System;
using BookLovers.Base.Domain.Events;
using Newtonsoft.Json;

namespace BookLovers.Librarians.Events.Librarians
{
    public class LibrarianSuspended : IEvent
    {
        public Guid Guid { get; }

        public Guid AggregateGuid { get; }

        public Guid ReaderGuid { get; }

        private LibrarianSuspended()
        {
        }

        [JsonConstructor]
        protected LibrarianSuspended(Guid guid, Guid aggregateGuid, Guid readerGuid)
        {
            this.Guid = guid;
            this.AggregateGuid = aggregateGuid;
            this.ReaderGuid = readerGuid;
        }

        public LibrarianSuspended(Guid aggregateGuid, Guid readerGuid)
        {
            this.Guid = Guid.NewGuid();
            this.AggregateGuid = aggregateGuid;
            this.ReaderGuid = readerGuid;
        }
    }
}