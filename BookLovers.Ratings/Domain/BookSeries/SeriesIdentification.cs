using System;
using BookLovers.Base.Domain.ValueObject;

namespace BookLovers.Ratings.Domain.BookSeries
{
    public class SeriesIdentification : ValueObject<SeriesIdentification>
    {
        public Guid SeriesGuid { get; private set; }

        public int SeriesId { get; private set; }

        private SeriesIdentification()
        {
        }

        public SeriesIdentification(Guid seriesGuid, int seriesId)
        {
            this.SeriesGuid = seriesGuid;
            this.SeriesId = seriesId;
        }

        protected override int GetHashCodeCore()
        {
            var hash = 17;

            hash = (hash * 23) + this.SeriesId.GetHashCode();
            hash = (hash * 23) + this.SeriesGuid.GetHashCode();

            return hash;
        }

        protected override bool EqualsCore(SeriesIdentification obj)
        {
            return this.SeriesId == obj.SeriesId && this.SeriesGuid == obj.SeriesGuid;
        }
    }
}