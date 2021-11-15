using System;
using BookLovers.Base.Domain.ValueObject;

namespace BookLovers.Publication.Domain.Books
{
    public class BookSeries : ValueObject<BookSeries>
    {
        public Guid? SeriesGuid { get; }

        public int? PositionInSeries { get; }

        private BookSeries()
        {
        }

        public BookSeries(Guid? seriesGuid, int? positionInSeries)
        {
            this.SeriesGuid = seriesGuid ?? Guid.Empty;
            this.PositionInSeries = positionInSeries.GetValueOrDefault();
        }

        protected override bool EqualsCore(BookSeries obj)
        {
            return this.SeriesGuid == obj.SeriesGuid && this.PositionInSeries == obj.PositionInSeries;
        }

        protected override int GetHashCodeCore()
        {
            var hash = 17;

            hash = (hash * 23) + this.SeriesGuid.GetHashCode();
            hash = (hash * 23) + this.PositionInSeries.GetHashCode();

            return hash;
        }
    }
}