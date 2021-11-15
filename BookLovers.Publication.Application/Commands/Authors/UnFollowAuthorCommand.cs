using System;
using BookLovers.Base.Infrastructure.Commands;

namespace BookLovers.Publication.Application.Commands.Authors
{
    public class UnFollowAuthorCommand : ICommand
    {
        public Guid AuthorGuid { get; }

        public UnFollowAuthorCommand(Guid authorGuid)
        {
            this.AuthorGuid = authorGuid;
        }
    }
}