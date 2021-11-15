using AutoMapper;
using BookLovers.Base.Infrastructure;
using BookLovers.Librarians.Domain.PromotionWaiters;
using BookLovers.Librarians.Infrastructure.Persistence.ReadModels;
using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Threading.Tasks;

namespace BookLovers.Librarians.Infrastructure.Persistence.Repositories
{
    public class PromotionWaiterRepository : IPromotionWaiterRepository, IRepository<PromotionWaiter>
    {
        private readonly LibrariansContext _context;
        private readonly IMapper _mapper;

        public PromotionWaiterRepository(LibrariansContext context, IMapper mapper)
        {
            this._context = context;
            this._mapper = mapper;
        }

        public async Task<PromotionWaiter> GetAsync(Guid aggregateGuid)
        {
            var promotionWaiter =
                await this._context.PromotionWaiters.SingleOrDefaultAsync(p => p.Guid == aggregateGuid);

            return this._mapper.Map<PromotionWaiterReadModel, PromotionWaiter>(promotionWaiter);
        }

        public async Task<PromotionWaiter> GetPromotionWaiterByReaderGuid(
            Guid readerGuid)
        {
            var promotionWaiter =
                await this._context.PromotionWaiters.SingleOrDefaultAsync(p => p.ReaderGuid == readerGuid);

            return this._mapper.Map<PromotionWaiterReadModel, PromotionWaiter>(promotionWaiter);
        }

        public async Task CommitChangesAsync(PromotionWaiter aggregate)
        {
            var promotionWaiter =
                await this._context.PromotionWaiters.SingleOrDefaultAsync(p => p.ReaderGuid == aggregate.ReaderGuid);

            var mapped = this._mapper.Map(aggregate, promotionWaiter);

            this._context.PromotionWaiters.AddOrUpdate(p => p.Id, mapped);

            await this._context.SaveChangesAsync();
        }
    }
}