using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BookLovers.Base.Infrastructure;
using BookLovers.Ratings.Domain.BookSeries;
using BookLovers.Ratings.Infrastructure.Persistence.ReadModels;

namespace BookLovers.Ratings.Infrastructure.Persistence.Repositories
{
    public class SeriesRepository : ISeriesRepository, IRepository<Series>
    {
        private readonly RatingsContext _context;
        private readonly IMapper _mapper;

        public SeriesRepository(RatingsContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Series> GetAsync(Guid aggregateGuid)
        {
            var series = await _context.Series
                .Include(p => p.Books.Select(s => s.Ratings))
                .SingleOrDefaultAsync(p => p.Guid == aggregateGuid);

            return _mapper.Map<SeriesReadModel, Series>(series);
        }

        public async Task CommitChangesAsync(Series aggregate)
        {
            var readModel = await _context.Series
                .Include(p => p.Books.Select(s => s.Ratings))
                .SingleOrDefaultAsync(p => p.SeriesGuid == aggregate.Identification.SeriesGuid);

            if (readModel == null)
                await AddNewSeriesReadModelAsync(aggregate);
            else
            {
                var seriesBooks = await GetSeriesBooks(aggregate);
                await UpdateSeriesReadModelAsync(aggregate, readModel, seriesBooks);
            }
        }

        public async Task<Series> GetBySeriesGuidAsync(Guid seriesGuid)
        {
            var series = await _context.Series
                .Include(p => p.Books.Select(s => s.Ratings)).SingleOrDefaultAsync(p => p.SeriesGuid == seriesGuid);

            return _mapper.Map<SeriesReadModel, Series>(series);
        }

        public async Task<Series> GetByIdAsync(int seriesId)
        {
            var series = await _context.Series
                .Include(p => p.Books.Select(s => s.Ratings))
                .SingleOrDefaultAsync(p => p.SeriesId == seriesId);

            return _mapper.Map<SeriesReadModel, Series>(series);
        }

        public async Task<IEnumerable<Series>> GetSeriesWithBookAsync(Guid bookGuid)
        {
            var series = await _context.Series
                .Include(p => p.Books.Select(s => s.Ratings))
                .Where(p => p.Books.Any(a => a.BookGuid == bookGuid))
                .ToListAsync();

            return _mapper.Map<List<SeriesReadModel>, List<Series>>(series);
        }

        private async Task UpdateSeriesReadModelAsync(
            Series series,
            SeriesReadModel seriesReadModel,
            List<BookReadModel> books)
        {
            seriesReadModel.Status = series.Status;
            seriesReadModel.Books = books;
            seriesReadModel.Average = series.Average();
            seriesReadModel.RatingsCount = series.RatingsCount();

            _context.Series.AddOrUpdate(p => p.SeriesGuid, seriesReadModel);

            await _context.SaveChangesAsync();
        }

        private async Task AddNewSeriesReadModelAsync(Series series)
        {
            _context.Series.Add(new SeriesReadModel()
            {
                Guid = series.Guid,
                SeriesGuid = series.Identification.SeriesGuid,
                SeriesId = series.Identification.SeriesId,
                Status = series.Status,
                Average = series.Average(),
                RatingsCount = series.RatingsCount(),
                Books = new List<BookReadModel>()
            });

            await _context.SaveChangesAsync();
        }

        private async Task<List<BookReadModel>> GetSeriesBooks(Series series)
        {
            var books = new List<BookReadModel>();

            foreach (var book in series.Books)
            {
                var seriesBook = await _context.Books.Include(p => p.Authors)
                    .SingleAsync(p => p.BookGuid == book.Identification.BookGuid);

                books.Add(seriesBook);
            }

            return books;
        }
    }
}