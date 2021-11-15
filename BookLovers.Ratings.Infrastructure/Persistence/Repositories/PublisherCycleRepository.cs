using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BookLovers.Base.Infrastructure;
using BookLovers.Ratings.Domain.PublisherCycles;
using BookLovers.Ratings.Infrastructure.Persistence.ReadModels;

namespace BookLovers.Ratings.Infrastructure.Persistence.Repositories
{
    internal class PublisherCycleRepository : IPublisherCycleRepository, IRepository<PublisherCycle>
    {
        private readonly RatingsContext _context;
        private readonly IMapper _mapper;

        public PublisherCycleRepository(RatingsContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PublisherCycle> GetByCycleGuidAsync(Guid cycleGuid)
        {
            var cycle = await _context.PublisherCycles
                .AsNoTracking()
                .Include(p => p.Books.Select(s => s.Ratings))
                .SingleOrDefaultAsync(p => p.PublisherCycleGuid == cycleGuid);

            return _mapper.Map<PublisherCycleReadModel, PublisherCycle>(cycle);
        }

        public async Task<PublisherCycle> GetByIdAsync(int cycleId)
        {
            var cycle = await _context.PublisherCycles
                .AsNoTracking().Include(p => p.Books.Select(s => s.Ratings))
                .SingleOrDefaultAsync(p => p.PublisherCycleId == cycleId);

            return _mapper.Map<PublisherCycleReadModel, PublisherCycle>(cycle);
        }

        public async Task<IEnumerable<PublisherCycle>> GetCyclesWithBookAsync(
            Guid bookGuid)
        {
            var cycles = await _context
                .PublisherCycles.AsNoTracking().Include(p => p.Books.Select(s => s.Ratings))
                .Where(p => p.Books.Any(a => a.BookGuid == bookGuid)).ToListAsync();

            return _mapper.Map<List<PublisherCycleReadModel>, List<PublisherCycle>>(cycles);
        }

        public async Task<PublisherCycle> GetAsync(Guid aggregateGuid)
        {
            var cycle = await _context.PublisherCycles
                .Include(p => p.Books.Select(s => s.Ratings)).SingleOrDefaultAsync(p => p.Guid == aggregateGuid);

            return _mapper.Map<PublisherCycleReadModel, PublisherCycle>(cycle);
        }

        public async Task CommitChangesAsync(PublisherCycle aggregate)
        {
            var cycle = await _context.PublisherCycles
                .Include(p => p.Books.Select(s => s.Ratings))
                .SingleOrDefaultAsync(p => p.PublisherCycleGuid == aggregate.Identification.CycleGuid);

            if (cycle == null)
                await AddNewPublisherReadModelAsync(aggregate);
            else
                await UpdatePublisherReadModelAsync(aggregate, cycle);
        }

        private async Task UpdatePublisherReadModelAsync(
            PublisherCycle publisherCycle,
            PublisherCycleReadModel readModel)
        {
            readModel.Status = publisherCycle.Status;
            readModel.Books = await GetPublisherBooks(publisherCycle);
            readModel.Average = publisherCycle.Average();
            readModel.RatingsCount = publisherCycle.RatingsCount();

            _context.PublisherCycles.AddOrUpdate(p => p.PublisherCycleGuid, readModel);

            await _context.SaveChangesAsync();
        }

        private async Task AddNewPublisherReadModelAsync(PublisherCycle publisherCycle)
        {
            _context.PublisherCycles.Add(new PublisherCycleReadModel()
            {
                Guid = publisherCycle.Guid,
                PublisherCycleGuid = publisherCycle.Identification.CycleGuid,
                PublisherCycleId = publisherCycle.Identification.CycleId,
                Status = publisherCycle.Status,
                Average = publisherCycle.Average(),
                RatingsCount = publisherCycle.RatingsCount(),
                Books = new List<BookReadModel>()
            });

            await _context.SaveChangesAsync();
        }

        private async Task<List<BookReadModel>> GetPublisherBooks(
            PublisherCycle publisherCycle)
        {
            var books = new List<BookReadModel>();

            foreach (var book in publisherCycle.Books)
            {
                var cycleBook = await _context.Books.Include(p => p.Authors)
                    .SingleAsync(p => p.BookGuid == book.Identification.BookGuid);

                books.Add(cycleBook);
            }

            return books;
        }
    }
}