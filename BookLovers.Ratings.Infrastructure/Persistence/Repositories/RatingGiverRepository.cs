using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BookLovers.Base.Infrastructure;
using BookLovers.Ratings.Domain;
using BookLovers.Ratings.Domain.RatingGivers;
using BookLovers.Ratings.Infrastructure.Persistence.ReadModels;

namespace BookLovers.Ratings.Infrastructure.Persistence.Repositories
{
    public class RatingGiverRepository : IRatingGiverRepository, IRepository<RatingGiver>
    {
        private readonly RatingsContext _context;
        private readonly IMapper _mapper;

        public RatingGiverRepository(RatingsContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<RatingGiver> GetAsync(Guid aggregateGuid)
        {
            var reader = await _context.Readers
                .SingleOrDefaultAsync(p => p.Guid == aggregateGuid);

            var ratings = await _context.Ratings
                .Where(p => p.ReaderId == reader.ReaderId).ToListAsync();

            var ratingGiver = _mapper.Map<ReaderReadModel, RatingGiver>(reader);

            foreach (var source in ratings)
                ratingGiver.AddRating(_mapper.Map<RatingReadModel, Rating>(source));

            return ratingGiver;
        }

        public async Task CommitChangesAsync(RatingGiver aggregate)
        {
            var ratingGiver = await _context.Readers.SingleOrDefaultAsync(p => p.Guid == aggregate.Guid);

            _context.Readers.AddOrUpdate(p => p.ReaderId, _mapper.Map(aggregate, ratingGiver));

            await _context.SaveChangesAsync();
        }

        public async Task<RatingGiver> GetRatingGiverByReaderGuid(Guid readerGuid)
        {
            var ratingGiver = await _context.Readers.SingleOrDefaultAsync(p => p.ReaderGuid == readerGuid);

            return _mapper.Map<ReaderReadModel, RatingGiver>(ratingGiver);
        }

        public async Task<RatingGiver> GetById(int readerId)
        {
            var ratingGiver = await _context.Readers.SingleOrDefaultAsync(p => p.ReaderId == readerId);

            return _mapper.Map<ReaderReadModel, RatingGiver>(ratingGiver);
        }
    }
}