using System;
using BookLovers.Base.Infrastructure.Commands;

namespace BookLovers.Readers.Application.Commands.Reviews
{
    public class AddSpoilerTagCommand : ICommand
    {
        public Guid ReviewGuid { get; }

        public AddSpoilerTagCommand(Guid reviewGuid)
        {
            ReviewGuid = reviewGuid;
        }
    }
}