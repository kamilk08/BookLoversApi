using System;
using BookLovers.Base.Domain.ValueObject;

namespace BookLovers.Publication.Domain.SeriesCycle
{
    public class SeriesBook : ValueObject<SeriesBook>, IComparable<SeriesBook>
    {
        public Guid BookGuid { get; }

        public int Position { get; }

        private SeriesBook()
        {
        }

        public SeriesBook(Guid bookGuid, int position)
        {
            this.BookGuid = bookGuid;
            this.Position = position;
        }

        public SeriesBook ChangePosition(int position)
        {
            return new SeriesBook(this.BookGuid, position);
        }

        protected override bool EqualsCore(SeriesBook obj)
        {
            return this.BookGuid == obj.BookGuid && this.Position == obj.Position;
        }

        protected override int GetHashCodeCore()
        {
            var hash = 17;

            hash = (hash * 23) + this.Position.GetHashCode();
            hash = (hash * 23) + this.BookGuid.GetHashCode();

            return hash;
        }

        public int CompareTo(SeriesBook other)
        {
            if (ReferenceEquals(this, other)) return 0;
            if (ReferenceEquals(null, other)) return 1;

            var bookGuidComparison = BookGuid.CompareTo(other.BookGuid);
            if (bookGuidComparison != 0) return bookGuidComparison;

            return Position.CompareTo(other.Position);
        }
    }
}