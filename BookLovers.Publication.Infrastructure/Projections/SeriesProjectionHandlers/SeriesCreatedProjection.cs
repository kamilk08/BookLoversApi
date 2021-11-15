using AutoMapper;
using BookLovers.Base.Infrastructure.Events.DomainEvents;
using BookLovers.Publication.Events.SeriesCycle;
using BookLovers.Publication.Infrastructure.Persistence;
using BookLovers.Publication.Infrastructure.Persistence.ReadModels;
using BookLovers.Publication.Infrastructure.Services;

namespace BookLovers.Publication.Infrastructure.Projections.SeriesProjectionHandlers
{
    internal class SeriesCreatedProjection : IProjectionHandler<SeriesCreated>, IProjectionHandler
    {
        private readonly PublicationsContext _context;
        private readonly IMapper _mapper;
        private readonly ReadContextAccessor _contextAccessor;

        public SeriesCreatedProjection(
            PublicationsContext context,
            IMapper mapper,
            ReadContextAccessor contextAccessor)
        {
            this._context = context;
            this._mapper = mapper;
            this._contextAccessor = contextAccessor;
        }

        public void Handle(SeriesCreated @event)
        {
            var series = _mapper.Map<SeriesCreated, SeriesReadModel>(@event);

            _context.Series.Add(series);

            _context.SaveChanges();

            _contextAccessor.AddReadModelId(@event.AggregateGuid, series.Id);
        }
    }
}