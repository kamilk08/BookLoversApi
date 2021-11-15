using System;
using BookLovers.Base.Domain.Events;

namespace BookLovers.Publication.Events.Book
{
    public class CoverChanged : IEvent
    {
        public Guid Guid { get; private set; }

        public Guid AggregateGuid { get; private set; }

        public string PictureSource { get; private set; }

        public int CoverType { get; private set; }

        private CoverChanged()
        {
        }

        private CoverChanged(Guid aggregateGuid, int coverType, string pictureSource)
        {
            this.Guid = Guid.NewGuid();
            this.AggregateGuid = aggregateGuid;
            this.CoverType = coverType;
            this.PictureSource = pictureSource;
        }

        public static CoverChanged Initialize()
        {
            return new CoverChanged();
        }

        public CoverChanged WithAggregate(Guid aggregateGuid)
        {
            return new CoverChanged(aggregateGuid, this.CoverType, this.PictureSource);
        }

        public CoverChanged WithCover(int coverTypeId, string pictureSource)
        {
            return new CoverChanged(this.AggregateGuid, coverTypeId, pictureSource);
        }

        public CoverChanged IsAdded(bool isAdded)
        {
            return new CoverChanged(this.AggregateGuid, this.CoverType, this.PictureSource);
        }
    }
}