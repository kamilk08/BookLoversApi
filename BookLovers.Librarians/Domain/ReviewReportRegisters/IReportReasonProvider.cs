namespace BookLovers.Librarians.Domain.ReviewReportRegisters
{
    public interface IReportReasonProvider
    {
        ReportReason GetReportReason(int reasonId);
    }
}