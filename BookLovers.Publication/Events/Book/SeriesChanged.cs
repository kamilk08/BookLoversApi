using System;
using BookLovers.Base.Domain.Events;

namespace BookLovers.Publication.Events.Book
{
    public class SeriesChanged : IEvent
    {
        public Guid SeriesGuid { get; private set; }

        public Guid OldSeriesGuid { get; private set; }

        public Guid Guid { get; private set; }

        public Guid AggregateGuid { get; private set; }

        public int? PositionInSeries { get; private set; }

        private SeriesChanged()
        {
        }

        private SeriesChanged(
            Guid bookGuid,
            Guid seriesGuid,
            int? positionInSeries,
            Guid oldSeriesGuid)
        {
            this.Guid = Guid.NewGuid();
            this.AggregateGuid = bookGuid;
            this.SeriesGuid = seriesGuid;
            this.OldSeriesGuid = oldSeriesGuid;
            this.PositionInSeries = positionInSeries;
        }

        public static SeriesChanged Initialize()
        {
            return new SeriesChanged();
        }

        public SeriesChanged WithAggregate(Guid aggregateGuid)
        {
            return new SeriesChanged(aggregateGuid, this.SeriesGuid,
                this.PositionInSeries, this.OldSeriesGuid);
        }

        public SeriesChanged WithNewSeries(Guid seriesGuid, int positionInSeries)
        {
            return new SeriesChanged(this.AggregateGuid, seriesGuid, positionInSeries, this.OldSeriesGuid);
        }

        public SeriesChanged WithOldSeries(Guid oldSeriesGuid)
        {
            return new SeriesChanged(this.AggregateGuid, this.SeriesGuid,
                this.PositionInSeries, oldSeriesGuid);
        }
    }
}