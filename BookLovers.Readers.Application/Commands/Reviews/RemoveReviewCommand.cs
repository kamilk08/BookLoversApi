using System;
using BookLovers.Base.Infrastructure.Commands;

namespace BookLovers.Readers.Application.Commands.Reviews
{
    public class RemoveReviewCommand : ICommand
    {
        public Guid ReviewGuid { get; private set; }

        public RemoveReviewCommand(Guid reviewGuid)
        {
            ReviewGuid = reviewGuid;
        }
    }
}