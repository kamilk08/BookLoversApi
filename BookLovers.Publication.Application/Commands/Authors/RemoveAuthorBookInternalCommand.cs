using System;
using BookLovers.Base.Infrastructure.Commands;

namespace BookLovers.Publication.Application.Commands.Authors
{
    internal class RemoveAuthorBookInternalCommand : IInternalCommand, ICommand
    {
        public Guid Guid { get; private set; }

        public Guid AuthorGuid { get; private set; }

        public Guid BookGuid { get; private set; }

        private RemoveAuthorBookInternalCommand()
        {
        }

        public RemoveAuthorBookInternalCommand(Guid authorGuid, Guid bookGuid)
        {
            this.Guid = Guid.NewGuid();
            this.AuthorGuid = authorGuid;
            this.BookGuid = bookGuid;
        }
    }
}