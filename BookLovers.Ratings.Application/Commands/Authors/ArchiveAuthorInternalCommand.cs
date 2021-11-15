using System;
using BookLovers.Base.Infrastructure.Commands;

namespace BookLovers.Ratings.Application.Commands.Authors
{
    internal class ArchiveAuthorInternalCommand : IInternalCommand, ICommand
    {
        public Guid AuthorGuid { get; private set; }

        public Guid Guid { get; private set; }

        private ArchiveAuthorInternalCommand()
        {
        }

        public ArchiveAuthorInternalCommand(Guid authorGuid)
        {
            this.Guid = Guid.NewGuid();
            this.AuthorGuid = authorGuid;
        }
    }
}