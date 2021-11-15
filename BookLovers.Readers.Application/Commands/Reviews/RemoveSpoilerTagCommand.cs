using System;
using BookLovers.Base.Infrastructure.Commands;

namespace BookLovers.Readers.Application.Commands.Reviews
{
    public class RemoveSpoilerTagCommand : ICommand
    {
        public Guid ReviewGuid { get; }

        public RemoveSpoilerTagCommand(Guid reviewGuid)
        {
            ReviewGuid = reviewGuid;
        }
    }
}