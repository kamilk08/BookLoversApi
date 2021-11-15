using System.Transactions;

namespace BookLovers.Base.Infrastructure.Persistence
{
    public static class TransactionScopeFactory
    {
        public static TransactionScope CreateTransactionScope(
            IsolationLevel isolationLevel)
        {
            return new TransactionScope(
                TransactionScopeOption.Required,
                new TransactionOptions
                {
                    IsolationLevel = isolationLevel,
                    Timeout = TransactionManager.MaximumTimeout
                }, TransactionScopeAsyncFlowOption.Enabled);
        }
    }
}