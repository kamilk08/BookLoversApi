using System.Linq;
using AutoMapper;
using BookLovers.Base.Infrastructure.Events.DomainEvents;
using BookLovers.Publication.Events.PublisherCycles;
using BookLovers.Publication.Infrastructure.Persistence;
using BookLovers.Publication.Infrastructure.Persistence.ReadModels;
using BookLovers.Publication.Infrastructure.Services;

namespace BookLovers.Publication.Infrastructure.Projections.PublisherCyclesProjectionHandlers
{
    internal class CycleCreatedProjection :
        IProjectionHandler<PublisherCycleCreated>,
        IProjectionHandler
    {
        private readonly PublicationsContext _context;
        private readonly IMapper _mapper;
        private readonly ReadContextAccessor _readContextAccessor;

        public CycleCreatedProjection(
            PublicationsContext context,
            IMapper mapper,
            ReadContextAccessor readContextAccessor)
        {
            this._context = context;
            this._mapper = mapper;
            this._readContextAccessor = readContextAccessor;
        }

        public void Handle(PublisherCycleCreated @event)
        {
            var publisher = this._context.Publishers
                .Single(p => p.Guid == @event.PublisherGuid);
            var publisherCycle = this._mapper.Map<PublisherCycleCreated, PublisherCycleReadModel>(@event);

            publisherCycle.Publisher = publisher;

            this._context.PublisherCycles.Add(publisherCycle);

            this._context.SaveChanges();

            this._readContextAccessor.AddReadModelId(@event.AggregateGuid, publisherCycle.Id);
        }
    }
}