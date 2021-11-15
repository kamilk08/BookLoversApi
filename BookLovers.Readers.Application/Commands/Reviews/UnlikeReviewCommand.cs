using System;
using BookLovers.Base.Infrastructure.Commands;

namespace BookLovers.Readers.Application.Commands.Reviews
{
    public class UnlikeReviewCommand : ICommand
    {
        public Guid ReviewGuid { get; private set; }

        private UnlikeReviewCommand()
        {
        }

        public UnlikeReviewCommand(Guid reviewGuid)
        {
            ReviewGuid = reviewGuid;
        }
    }
}