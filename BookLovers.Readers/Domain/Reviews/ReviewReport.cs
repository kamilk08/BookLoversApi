using System;
using BookLovers.Base.Domain.ValueObject;

namespace BookLovers.Readers.Domain.Reviews
{
    public class ReviewReport : ValueObject<ReviewReport>
    {
        public Guid ReportedBy { get; }

        private ReviewReport()
        {
        }

        public ReviewReport(Guid reportedBy)
        {
            ReportedBy = reportedBy;
        }

        protected override int GetHashCodeCore()
        {
            return (17 * 23) + ReportedBy.GetHashCode();
        }

        protected override bool EqualsCore(ReviewReport obj)
        {
            return ReportedBy == obj.ReportedBy;
        }
    }
}