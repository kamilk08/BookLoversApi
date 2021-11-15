using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using BookLovers.Base.Domain.Aggregates;
using BookLovers.Librarians.Domain.ReviewReportRegisters.BusinessRules;
using BookLovers.Librarians.Domain.Tickets;

namespace BookLovers.Librarians.Domain.ReviewReportRegisters
{
    public class ReviewReportRegister : AggregateRoot
    {
        public Guid ReviewGuid { get; private set; }

        public SolvedBy SolvedBy { get; private set; }

        private ICollection<ReportRegisterItem> _reviewReports { get; set; } =
            new List<ReportRegisterItem>();

        public IReadOnlyList<ReportRegisterItem> ReviewReports =>
            this._reviewReports.ToList<ReportRegisterItem>();

        private ReviewReportRegister()
        {
        }

        public ReviewReportRegister(Guid aggregateGuid, Guid reviewGuid)
        {
            this.Guid = aggregateGuid;
            this.ReviewGuid = reviewGuid;
            this.SolvedBy = new SolvedBy(Guid.Empty);
            this.Status = AggregateStatus.Active.Value;
        }

        public void AddReportToRegister(ReportRegisterItem reportRegisterItem)
        {
            this.CheckBusinessRules(new AddReviewReportRules(this, reportRegisterItem));

            this._reviewReports.Add(reportRegisterItem);
        }

        internal void SolveAllReports(Guid librarianGuid) =>
            this.SolvedBy = new SolvedBy(librarianGuid);

        public bool HasReportFrom(Guid readerGuid)
        {
            return this._reviewReports.Any(a => a.ReportedBy == readerGuid);
        }

        public class Relations
        {
            public const string ReviewReports = "_reviewReports";

            public static Expression<Func<ReviewReportRegister, ICollection<ReportRegisterItem>>> Reports
                = register => register._reviewReports;
        }
    }
}