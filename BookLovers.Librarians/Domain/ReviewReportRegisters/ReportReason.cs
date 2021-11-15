using BookLovers.Base.Domain.Extensions;

namespace BookLovers.Librarians.Domain.ReviewReportRegisters
{
    public class ReportReason : Enumeration
    {
        public static readonly ReportReason AbusiveContent = new ReportReason(1, "Abusive content");
        public static readonly ReportReason Spam = new ReportReason(2, nameof(Spam));
        public static readonly ReportReason ContainsThreats = new ReportReason(3, "Contains threats");

        protected ReportReason()
        {
        }

        public ReportReason(int value, string name)
            : base(value, name)
        {
        }
    }
}