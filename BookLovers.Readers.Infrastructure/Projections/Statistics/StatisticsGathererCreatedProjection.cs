using System.Linq;
using BookLovers.Base.Infrastructure.Events.DomainEvents;
using BookLovers.Readers.Domain.Statistics;
using BookLovers.Readers.Events.Statistics;
using BookLovers.Readers.Infrastructure.Persistence;
using BookLovers.Readers.Infrastructure.Persistence.ReadModels;

namespace BookLovers.Readers.Infrastructure.Projections.Statistics
{
    internal class StatisticsGathererCreatedProjection :
        IProjectionHandler<StatisticsGathererCreated>,
        IProjectionHandler
    {
        private readonly ReadersContext _context;

        public StatisticsGathererCreatedProjection(ReadersContext context)
        {
            this._context = context;
        }

        public void Handle(StatisticsGathererCreated @event)
        {
            var reader = this._context.Readers.Single(p => p.Guid == @event.ReaderGuid);

            this._context.Statistics.Add(new StatisticsReadModel()
            {
                Guid = @event.AggregateGuid,
                Reader = reader,
                ShelvesCount = CurrentShelves.DefaultShelvesValue
            });

            this._context.SaveChanges();
        }
    }
}