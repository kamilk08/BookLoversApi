using System;
using System.Threading.Tasks;
using BookLovers.Base.Infrastructure;

namespace BookLovers.Librarians.Domain.PromotionWaiters
{
    public interface IPromotionWaiterRepository : IRepository<PromotionWaiter>
    {
        Task<PromotionWaiter> GetPromotionWaiterByReaderGuid(Guid readerGuid);
    }
}