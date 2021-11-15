using System;
using BookLovers.Base.Infrastructure.Commands;

namespace BookLovers.Publication.Application.Commands.Authors
{
    public class FollowAuthorCommand : ICommand
    {
        public Guid AuthorGuid { get; }

        public FollowAuthorCommand(Guid authorGuid)
        {
            this.AuthorGuid = authorGuid;
        }
    }
}