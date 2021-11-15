using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Transactions;
using BookLovers.Base.Domain.Aggregates;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Events.DomainEvents;
using BookLovers.Base.Infrastructure.Persistence;
using Ninject;

namespace BookLovers.Ratings.Infrastructure.Root
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IDomainEventDispatcher _dispatcher;

        public UnitOfWork(IDomainEventDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        public Task<TAggregate> GetAsync<TAggregate>(Guid aggregateGuid)
            where TAggregate : class, IRoot
        {
            return CompositionRoot.Kernel.Get<IRepository<TAggregate>>().GetAsync(aggregateGuid);
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
            var repository = CompositionRoot.Kernel.Get(typeof(IRepository<TAggregate>)) as IRepository<TAggregate>;
            await repository.CommitChangesAsync(aggregate);

            var uncommittedEvents = aggregate.GetUncommittedEvents();
            if (!dispatchEvents)
                return;

            foreach (var @event in uncommittedEvents)
                await _dispatcher.Dispatch(@event);
        }

        private bool TransactionIsOpen()
        {
            return Transaction.Current != null;
        }
    }
}