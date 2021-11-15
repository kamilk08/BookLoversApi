using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BookLovers.Base.Infrastructure;

namespace BookLovers.Ratings.Domain.BookSeries
{
    public interface ISeriesRepository : IRepository<Series>
    {
        Task<Series> GetBySeriesGuidAsync(Guid seriesGuid);

        Task<Series> GetByIdAsync(int seriesId);

        Task<IEnumerable<Series>> GetSeriesWithBookAsync(Guid bookGuid);
    }
}