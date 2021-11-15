using BookLovers.Base.Domain.ValueObject;

namespace BookLovers.Readers.Domain.Reviews
{
    public class ReviewSpoiler : ValueObject<ReviewSpoiler>
    {
        public bool MarkedAsSpoilerByReader { get; }

        public bool MarkedByOtherReaders { get; }

        private ReviewSpoiler()
        {
        }

        public ReviewSpoiler(bool markedAsSpoilerByReader, bool markedByOtherReaders)
        {
            MarkedAsSpoilerByReader = markedAsSpoilerByReader;
            MarkedByOtherReaders = markedByOtherReaders;
        }

        public ReviewSpoiler MarkedByReader()
        {
            return new ReviewSpoiler(true, MarkedByOtherReaders);
        }

        public ReviewSpoiler MarkedByOthers()
        {
            return new ReviewSpoiler(MarkedAsSpoilerByReader, true);
        }

        public ReviewSpoiler UnMarkedByReader()
        {
            return new ReviewSpoiler(false, MarkedByOtherReaders);
        }

        public ReviewSpoiler UnMarkedByOthers()
        {
            return new ReviewSpoiler(MarkedAsSpoilerByReader, false);
        }

        protected override int GetHashCodeCore()
        {
            return (17 * 23) + MarkedAsSpoilerByReader.GetHashCode();
        }

        protected override bool EqualsCore(ReviewSpoiler obj)
        {
            return MarkedAsSpoilerByReader == obj.MarkedAsSpoilerByReader;
        }
    }
}