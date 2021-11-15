using System;
using BookLovers.Base.Infrastructure.Commands;

namespace BookLovers.Publication.Application.Commands.Authors
{
    public class ArchiveAuthorCommand : ICommand
    {
        public Guid AuthorGuid { get; }

        public ArchiveAuthorCommand(Guid authorGuid)
        {
            this.AuthorGuid = authorGuid;
        }
    }
}