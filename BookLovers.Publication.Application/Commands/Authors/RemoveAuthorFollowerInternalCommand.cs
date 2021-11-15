using System;
using BookLovers.Base.Infrastructure.Commands;

namespace BookLovers.Publication.Application.Commands.Authors
{
    internal class RemoveAuthorFollowerInternalCommand : IInternalCommand, ICommand
    {
        public Guid Guid { get; private set; }

        public Guid AuthorGuid { get; private set; }

        public Guid ReaderGuid { get; private set; }

        private RemoveAuthorFollowerInternalCommand()
        {
        }

        public RemoveAuthorFollowerInternalCommand(Guid authorGuid, Guid readerGuid)
        {
            this.Guid = Guid.NewGuid();
            this.AuthorGuid = authorGuid;
            this.ReaderGuid = readerGuid;
        }
    }
}