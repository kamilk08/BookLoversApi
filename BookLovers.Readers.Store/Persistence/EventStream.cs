using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using BookLovers.Base.Domain.Aggregates;
using BookLovers.Base.Infrastructure.Persistence;
using Newtonsoft.Json;

namespace BookLovers.Readers.Store.Persistence
{
    public class EventStream : IEventStream
    {
        private readonly ReadersStoreContext _context;

        public EventStream(ReadersStoreContext context)
        {
            _context = context;
        }

        public Task<List<IEventEntity>> GetEventStream(
            Guid aggregateGuid,
            int from,
            int to)
        {
            return _context.EventEntities
                .Where<IEventEntity>(p => p.AggregateGuid == aggregateGuid && p.Version >= from && p.Version <= to)
                .AsNoTracking()
                .ToListAsync();
        }

        public Task<List<IEventEntity>> GetEventStream(Guid aggregateGuid)
        {
            return _context.EventEntities
                .Where<IEventEntity>(p => p.AggregateGuid == aggregateGuid)
                .OrderBy(p => p.Version)
                .AsNoTracking()
                .ToListAsync();
        }

        public Task AppendToEventStream<TAggregate>(TAggregate aggregate) where TAggregate : IEventSourcedAggregateRoot
        {
            var uncommittedEvents = aggregate.GetUncommittedEvents();

            int committedVersion = aggregate.LastCommittedVersion;

            foreach (var @event in uncommittedEvents)
            {
                var type = @event.GetType();
                _context.EventEntities.Add(new EventEntity(aggregate.Guid, JsonConvert.SerializeObject(@event),
                    type.ToString(), type.Assembly.FullName)
                {
                    Version = ++committedVersion
                });
            }

            _context.SaveChanges();

            return Task.CompletedTask;
        }

        public Task ClearEventStreamAsync()
        {
            throw new NotImplementedException();
        }
    }
}