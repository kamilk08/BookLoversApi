using System;
using BookLovers.Base.Infrastructure.Commands;

namespace BookLovers.Publication.Application.Commands.Authors
{
    internal class AddAuthorFollowerInternalCommand : IInternalCommand, ICommand
    {
        public Guid Guid { get; private set; }

        public Guid AuthorGuid { get; private set; }

        public Guid ReaderGuid { get; private set; }

        private AddAuthorFollowerInternalCommand()
        {
        }

        public AddAuthorFollowerInternalCommand(Guid authorGuid, Guid readerGuid)
        {
            this.Guid = Guid.NewGuid();
            this.AuthorGuid = authorGuid;
            this.ReaderGuid = readerGuid;
        }
    }
}