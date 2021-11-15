using System;
using BookLovers.Base.Infrastructure.Commands;

namespace BookLovers.Readers.Application.Commands.Reviews
{
    internal class RemoveReviewInternalCommand : IInternalCommand, ICommand
    {
        public Guid Guid { get; private set; }

        public Guid ReviewGuid { get; private set; }

        public RemoveReviewInternalCommand()
        {
        }

        public RemoveReviewInternalCommand(Guid reviewGuid)
        {
            Guid = Guid.NewGuid();
            ReviewGuid = reviewGuid;
        }
    }
}