using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookLovers.Base.Domain.Aggregates;
using BookLovers.Base.Infrastructure.Persistence;
using Newtonsoft.Json;

namespace BaseTests.EventStore
{
    internal class InMemoryEventStream : IEventStream
    {
        private List<IEventEntity> _entities = new List<IEventEntity>();

        public Task<List<IEventEntity>> GetEventStream(Guid aggregateGuid, int @from, int to)
        {
            var task = _entities.Where<IEventEntity>(p =>
                    p.AggregateGuid == aggregateGuid && p.Version >= from && p.Version <= to)
                .ToList();

            return Task.FromResult(task);
        }

        public Task<List<IEventEntity>> GetEventStream(Guid aggregateGuid)
        {
            var task = _entities.Where<IEventEntity>(p => p.AggregateGuid == aggregateGuid)
                .OrderBy(p => p.Version)
                .ToList();

            return Task.FromResult(task);
        }

        public Task AppendToEventStream<TAggregate>(TAggregate aggregate)
            where TAggregate : IEventSourcedAggregateRoot
        {
            var @events = aggregate.GetUncommittedEvents();
            var aggregateVersion = aggregate.LastCommittedVersion;

            foreach (var @event in @events)
            {
                var eventType = @event.GetType();

                var eventEntity = new InMemoryEventEntity(
                    aggregate.Guid,
                    JsonConvert.SerializeObject(@event),
                    eventType.ToString(),
                    eventType.Assembly.FullName);

                eventEntity.Version = ++aggregateVersion;

                _entities.Add(eventEntity);
            }

            return Task.CompletedTask;
        }

        public Task ClearEventStreamAsync()
        {
            _entities.Clear();

            return Task.CompletedTask;
        }
    }
}