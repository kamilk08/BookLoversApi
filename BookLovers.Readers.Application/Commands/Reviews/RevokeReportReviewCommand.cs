using System;
using BookLovers.Base.Infrastructure.Commands;

namespace BookLovers.Readers.Application.Commands.Reviews
{
    public class RevokeReportReviewCommand : ICommand
    {
        public Guid ReviewGuid { get; }

        public RevokeReportReviewCommand(Guid reviewGuid)
        {
            ReviewGuid = reviewGuid;
        }
    }
}