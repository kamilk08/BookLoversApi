using System;
using BookLovers.Base.Domain.ValueObject;

namespace BookLovers.Librarians.Domain.ReviewReportRegisters
{
    public class ReportRegisterItem : ValueObject<ReportRegisterItem>
    {
        public Guid ReportedBy { get; private set; }

        public ReportReason ReportReason { get; private set; }

        private ReportRegisterItem()
        {
        }

        public ReportRegisterItem(Guid reportedBy, ReportReason reportReason)
        {
            this.ReportedBy = reportedBy;
            this.ReportReason = reportReason;
        }

        protected override int GetHashCodeCore()
        {
            return (17 * 23) + this.ReportedBy.GetHashCode();
        }

        protected override bool EqualsCore(ReportRegisterItem obj)
        {
            return this.ReportedBy == obj.ReportedBy;
        }
    }
}