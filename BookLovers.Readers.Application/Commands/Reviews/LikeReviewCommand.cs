using System;
using BookLovers.Base.Infrastructure.Commands;

namespace BookLovers.Readers.Application.Commands.Reviews
{
    public class LikeReviewCommand : ICommand
    {
        public Guid ReviewGuid { get; }

        public LikeReviewCommand(Guid reviewGuid)
        {
            ReviewGuid = reviewGuid;
        }
    }
}