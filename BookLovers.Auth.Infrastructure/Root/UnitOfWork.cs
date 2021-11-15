using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using BookLovers.Base.Domain.Aggregates;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Events.DomainEvents;
using BookLovers.Base.Infrastructure.Persistence;
using Ninject;

namespace BookLovers.Auth.Infrastructure.Root
{
    internal class UnitOfWork : IUnitOfWork
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

            foreach (var guid in guides)
            {
                var root = await GetAsync<TAggregate>(guid);

                list.Add(root);
            }

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
            var repo = CompositionRoot.Kernel.Get<IRepository<TAggregate>>();

            await repo.CommitChangesAsync(aggregate);

            var uncommittedEvents = aggregate.GetUncommittedEvents().ToList();

            if (dispatchEvents)
            {
                foreach (var @event in uncommittedEvents)
                    await _dispatcher.Dispatch(@event);
            }
        }

        private bool TransactionIsOpen()
        {
            return Transaction.Current != null;
        }
    }
}