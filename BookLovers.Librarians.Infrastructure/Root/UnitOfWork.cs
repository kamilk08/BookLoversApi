using BookLovers.Base.Domain.Aggregates;
using BookLovers.Base.Domain.Events;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Events.DomainEvents;
using BookLovers.Base.Infrastructure.Persistence;
using Ninject;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Transactions;

namespace BookLovers.Librarians.Infrastructure.Root
{
    internal class UnitOfWork : IUnitOfWork
    {
        private readonly IDomainEventDispatcher _eventDispatcher;

        public UnitOfWork(IDomainEventDispatcher eventDispatcher)
        {
            _eventDispatcher = eventDispatcher;
        }

        public Task<TAggregate> GetAsync<TAggregate>(Guid aggregateGuid) where TAggregate : class, IRoot
        {
            var repository = (IRepository<TAggregate>) CompositionRoot.Kernel.Get(typeof(IRepository<TAggregate>));

            return repository.GetAsync(aggregateGuid);
        }

        public async Task<List<TAggregate>> GetMultipleAsync<TAggregate>(List<Guid> guides)
            where TAggregate : class, IRoot
        {
            var roots = new List<TAggregate>();

            foreach (var guide in guides)
                roots.Add(await GetAsync<TAggregate>(guide));

            return roots;
        }

        public async Task CommitAsync<TAggregate>(TAggregate aggregate, bool dispatchEvents = true)
            where TAggregate : class, IRoot
        {
            if (IsTransactionIsOpen())
                await CommitAndDispatchEventsAsync(aggregate, dispatchEvents);
            else
            {
                using (var transaction = TransactionScopeFactory.CreateTransactionScope(IsolationLevel.ReadCommitted))
                {
                    await CommitAndDispatchEventsAsync(aggregate, dispatchEvents);
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
            var repository = (IRepository<TAggregate>) CompositionRoot.Kernel.Get(typeof(IRepository<TAggregate>));

            await (repository).CommitChangesAsync(aggregate);

            var uncommittedEvents = aggregate.GetUncommittedEvents();

            if (dispatchEvents)
                foreach (IEvent @event in uncommittedEvents)
                    await _eventDispatcher.Dispatch(@event);
        }

        private bool IsTransactionIsOpen()
        {
            return Transaction.Current != null;
        }
    }
}