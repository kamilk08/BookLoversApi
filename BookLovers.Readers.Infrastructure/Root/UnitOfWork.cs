using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using BookLovers.Base.Domain.Aggregates;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Events.DomainEvents;
using BookLovers.Base.Infrastructure.Persistence;

namespace BookLovers.Readers.Infrastructure.Root
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IEventStore _eventStore;
        private readonly IProjectionDispatcher _projectionDispatcher;
        private readonly IDomainEventDispatcher _eventDispatcher;

        public UnitOfWork(
            IEventStore eventStore,
            IProjectionDispatcher projectionDispatcher,
            IDomainEventDispatcher eventDispatcher)
        {
            this._eventStore = eventStore;
            this._projectionDispatcher = projectionDispatcher;
            this._eventDispatcher = eventDispatcher;
        }

        public Task<TAggregate> GetAsync<TAggregate>(Guid aggregateGuid)
            where TAggregate : class, IRoot
        {
            return this._eventStore.GetAsync<TAggregate>(aggregateGuid);
        }

        public async Task<List<TAggregate>> GetMultipleAsync<TAggregate>(List<Guid> guides)
            where TAggregate : class, IRoot
        {
            var list = new List<TAggregate>();

            foreach (var guid in guides)
                list.Add(await this.GetAsync<TAggregate>(guid));

            return list;
        }

        public async Task CommitAsync<TAggregate>(TAggregate aggregate, bool dispatchEvents = true)
            where TAggregate : class, IRoot
        {
            if (this.TransactionIsOpen())
            {
                await this.CommitAndDispatchEventsAsync(aggregate, dispatchEvents);
            }
            else
            {
                using (var transaction = TransactionScopeFactory.CreateTransactionScope(IsolationLevel.ReadCommitted))
                {
                    await this.CommitAndDispatchEventsAsync(aggregate, dispatchEvents);
                    transaction.Complete();
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

            await this._eventStore.StoreEvents(aggregate);

            foreach (var @event in uncommittedEvents)
                this._projectionDispatcher.Dispatch(@event);

            if (dispatchEvents)
                foreach (var @event in uncommittedEvents)
                    await this._eventDispatcher.Dispatch(@event);
        }

        private bool TransactionIsOpen() => Transaction.Current != null;
    }
}