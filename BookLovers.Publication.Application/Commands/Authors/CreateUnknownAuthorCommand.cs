using System;
using BookLovers.Base.Infrastructure.Commands;

namespace BookLovers.Publication.Application.Commands.Authors
{
    public class CreateUnknownAuthorCommand : ICommand
    {
        public Guid AuthorGuid { get; }

        public Guid AddedByGuid { get; }

        public CreateUnknownAuthorCommand(Guid authorGuid, Guid addedByGuid)
        {
            this.AuthorGuid = authorGuid;
            this.AddedByGuid = addedByGuid;
        }
    }
}