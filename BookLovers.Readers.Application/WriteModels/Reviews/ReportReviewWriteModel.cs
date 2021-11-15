using System;

namespace BookLovers.Readers.Application.WriteModels.Reviews
{
    public class ReportReviewWriteModel
    {
        public Guid ReviewGuid { get; set; }

        public int ReportReasonId { get; set; }
    }
}