using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using BookLovers.Base.Domain.Aggregates;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Events.DomainEvents;
using BookLovers.Base.Infrastructure.Persistence;

namespace BookLovers.Bookcases.Infrastructure.Root
{
    internal class UnitOfWork : IUnitOfWork
    {
        private readonly IEventStore _eventStore;
        private readonly IDomainEventDispatcher _eventDispatcher;
        private readonly IProjectionDispatcher _projectionDispatcher;

        public UnitOfWork(
            IEventStore eventStore,
            IDomainEventDispatcher eventDispatcher,
            IProjectionDispatcher projectionDispatcher)
        {
            _eventStore = eventStore;
            _eventDispatcher = eventDispatcher;
            _projectionDispatcher = projectionDispatcher;
        }

        public Task<TAggregate> GetAsync<TAggregate>(Guid aggregateGuid)
            where TAggregate : class, IRoot
        {
            return _eventStore.GetAsync<TAggregate>(aggregateGuid);
        }

        public async Task<List<TAggregate>> GetMultipleAsync<TAggregate>(List<Guid> guides)
            where TAggregate : class, IRoot
        {
            var list = new List<TAggregate>();

            foreach (var guide in guides)
                list.Add(await GetAsync<TAggregate>(guide));

            return list;
        }

        public async Task CommitAsync<TAggregate>(TAggregate aggregate, bool dispatchEvents = true)
            where TAggregate : class, IRoot
        {
            if (TransactionIsOpen())
            {
                await CommitAndDispatchEventsAsync(aggregate, dispatchEvents);
            }
            else
            {
                using (var scope =
                    TransactionScopeFactory.CreateTransactionScope(IsolationLevel.ReadCommitted))
                {
                    await CommitAndDispatchEventsAsync(aggregate, dispatchEvents);
                    scope.Complete();
                }
            }

            aggregate.CommitEvents();
        }

        private async Task CommitAndDispatchEventsAsync<TAggregate>(
            TAggregate aggregate,
            bool dispatchEvents)
            where TAggregate : class, IRoot
        {
            var uncommittedEvents = aggregate.GetUncommittedEvents().ToList();

            await _eventStore.StoreEvents(aggregate);

            foreach (var @event in uncommittedEvents)
                _projectionDispatcher.Dispatch(@event);

            if (dispatchEvents)
                foreach (var @event in uncommittedEvents)
                    await _eventDispatcher.Dispatch(@event);
        }

        private bool TransactionIsOpen()
        {
            return Transaction.Current != null;
        }
    }
}