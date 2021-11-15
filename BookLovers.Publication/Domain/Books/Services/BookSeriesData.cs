using System;

namespace BookLovers.Publication.Domain.Books.Services
{
    public class BookSeriesData
    {
        public Guid SeriesGuid { get; }

        public int PositionInSeries { get; }

        public BookSeriesData(Guid? seriesGuid, int? positionInSeries)
        {
            this.SeriesGuid = seriesGuid.GetValueOrDefault();
            this.PositionInSeries = positionInSeries.GetValueOrDefault();
        }
    }
}