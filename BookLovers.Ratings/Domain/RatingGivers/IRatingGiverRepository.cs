using System;
using System.Threading.Tasks;
using BookLovers.Base.Infrastructure;

namespace BookLovers.Ratings.Domain.RatingGivers
{
    public interface IRatingGiverRepository : IRepository<RatingGiver>
    {
        Task<RatingGiver> GetRatingGiverByReaderGuid(Guid readerGuid);

        Task<RatingGiver> GetById(int readerId);
    }
}