using System;
using BookLovers.Base.Infrastructure.Commands;

namespace BookLovers.Librarians.Application.Commands.ReviewRegisters
{
    internal class AddReviewReportToRegisterInternalCommand : IInternalCommand, ICommand
    {
        public Guid Guid { get; private set; }

        public Guid ReviewGuid { get; private set; }

        public Guid ReaderGuid { get; private set; }

        public int ReportReasonId { get; private set; }

        private AddReviewReportToRegisterInternalCommand()
        {
        }

        public AddReviewReportToRegisterInternalCommand(
            Guid reviewGuid,
            Guid readerGuid,
            int reportReasonId)
        {
            this.Guid = Guid.NewGuid();
            this.ReviewGuid = reviewGuid;
            this.ReaderGuid = readerGuid;
            this.ReportReasonId = reportReasonId;
        }
    }
}