using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BookLovers.Base.Infrastructure;
using BookLovers.Ratings.Domain.Publisher;
using BookLovers.Ratings.Infrastructure.Persistence.ReadModels;

namespace BookLovers.Ratings.Infrastructure.Persistence.Repositories
{
    public class PublisherRepository : IPublisherRepository, IRepository<Publisher>
    {
        private readonly RatingsContext _context;
        private readonly IMapper _mapper;

        public PublisherRepository(RatingsContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Publisher> GetAsync(
            Guid aggregateGuid)
        {
            var publisher = await _context.Publishers.Include(p => p.Books.Select(s => s.Ratings))
                .Include(p => p.PublisherCycles).SingleOrDefaultAsync(p => p.Guid == aggregateGuid);

            return _mapper.Map<PublisherReadModel, Publisher>(publisher);
        }

        public async Task CommitChangesAsync(Publisher aggregate)
        {
            var publisher = await _context.Publishers
                .Include(p => p.Books.Select(s => s.Ratings))
                .Include(p => p.PublisherCycles)
                .SingleOrDefaultAsync(p => p.PublisherGuid == aggregate.Identification.PublisherGuid);

            if (publisher == null)
                await AddNewPublisherReadModelAsync(aggregate);
            else
                await UpdatePublisherReadModelAsync(aggregate, publisher);
        }

        public async Task<Publisher> GetByPublisherGuidAsync(
            Guid publisherGuid)
        {
            var publisher = await _context.Publishers
                .Include(p => p.Books.Select(s => s.Ratings)).Include(p => p.PublisherCycles)
                .SingleOrDefaultAsync(p => p.PublisherGuid == publisherGuid);

            return _mapper.Map<PublisherReadModel, Publisher>(publisher);
        }

        public async Task<Publisher> GetByIdAsync(
            int publisherId)
        {
            var publisher = await _context.Publishers
                .Include(p => p.Books.Select(s => s.Ratings)).Include(p => p.PublisherCycles)
                .SingleOrDefaultAsync(p => p.PublisherId == publisherId);

            return _mapper.Map<PublisherReadModel, Publisher>(publisher);
        }

        public async Task<Publisher> GetPublisherWithBookAsync(
            Guid bookGuid)
        {
            var publisher = await _context.Publishers
                .Include(p => p.Books.Select(s => s.Ratings)).Include(p => p.PublisherCycles)
                .SingleOrDefaultAsync(p => p.Books.Any(a => a.BookGuid == bookGuid));

            return _mapper.Map<PublisherReadModel, Publisher>(publisher);
        }

        private async Task UpdatePublisherReadModelAsync(
            Publisher publisher,
            PublisherReadModel readModel)
        {
            readModel.Status = publisher.Status;
            readModel.Books = await GetPublisherBooks(publisher);
            readModel.PublisherCycles = await GetPublisherCycles(publisher);
            readModel.Average = publisher.Average();
            readModel.RatingsCount = publisher.RatingsCount();

            _context.Publishers.AddOrUpdate(p => p.PublisherGuid, readModel);

            await _context.SaveChangesAsync();
        }

        private async Task AddNewPublisherReadModelAsync(Publisher publisher)
        {
            _context.Publishers.Add(new PublisherReadModel()
            {
                Guid = publisher.Guid,
                PublisherGuid = publisher.Identification.PublisherGuid,
                PublisherId = publisher.Identification.PublisherId,
                Status = publisher.Status,
                Books = new List<BookReadModel>(),
                PublisherCycles = new List<PublisherCycleReadModel>(),
                Average = publisher.Average(),
                RatingsCount = publisher.RatingsCount()
            });

            await _context.SaveChangesAsync();
        }

        private async Task<List<BookReadModel>> GetPublisherBooks(
            Publisher publisher)
        {
            var books = new List<BookReadModel>();

            foreach (var book in publisher.Books)
            {
                var publisherBook = await _context.Books.Include(p => p.Authors)
                    .SingleAsync(p => p.BookGuid == book.Identification.BookGuid);

                books.Add(publisherBook);
            }

            return books;
        }

        private async Task<List<PublisherCycleReadModel>> GetPublisherCycles(
            Publisher publisher)
        {
            var cycles = new List<PublisherCycleReadModel>();

            foreach (var publisherCycle in publisher.PublisherCycles)
            {
                var cycleBook = await _context.PublisherCycles.Include(p => p.Books)
                    .SingleAsync(p => p.PublisherCycleGuid == publisherCycle.Identification.CycleGuid);

                cycles.Add(cycleBook);
            }

            return cycles;
        }
    }
}