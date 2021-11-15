using System;
using BookLovers.Base.Domain.Events;
using Newtonsoft.Json;

namespace BookLovers.Librarians.Events.Librarians
{
    public class LibrarianCreated : IEvent
    {
        public Guid Guid { get; }

        public Guid AggregateGuid { get; }

        public Guid ReaderGuid { get; }

        private LibrarianCreated()
        {
        }

        [JsonConstructor]
        protected LibrarianCreated(Guid guid, Guid aggregateGuid, Guid readerGuid)
        {
            this.Guid = guid;
            this.AggregateGuid = aggregateGuid;
            this.ReaderGuid = readerGuid;
        }

        public LibrarianCreated(Guid aggregateGuid, Guid readerGuid)
        {
            this.Guid = Guid.NewGuid();
            this.AggregateGuid = aggregateGuid;
            this.ReaderGuid = readerGuid;
        }
    }
}