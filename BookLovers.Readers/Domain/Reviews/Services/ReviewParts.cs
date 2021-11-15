using System;

namespace BookLovers.Readers.Domain.Reviews.Services
{
    public class ReviewParts
    {
        public Guid ReaderGuid { get; private set; }

        public Guid ReviewGuid { get; private set; }

        public Guid BookGuid { get; private set; }

        public string Content { get; private set; }

        public DateTime ReviewDate { get; private set; }

        public DateTime EditDate { get; private set; }

        public bool MarkedAsSpoiler { get; private set; }

        private ReviewParts()
        {
        }

        public static ReviewParts Initialize() => new ReviewParts();

        public ReviewParts WithGuid(Guid reviewGuid)
        {
            ReviewGuid = reviewGuid;

            return this;
        }

        public ReviewParts AddedBy(Guid addedBy)
        {
            ReaderGuid = addedBy;

            return this;
        }

        public ReviewParts WitBook(Guid bookGuid)
        {
            BookGuid = bookGuid;

            return this;
        }

        public ReviewParts WithContent(string content)
        {
            Content = content;

            return this;
        }

        public ReviewParts WithDates(DateTime reviewDate, DateTime editDate)
        {
            ReviewDate = reviewDate;
            EditDate = editDate;

            return this;
        }

        public ReviewParts HasSpoilers(bool flag = false)
        {
            MarkedAsSpoiler = flag;

            return this;
        }
    }
}