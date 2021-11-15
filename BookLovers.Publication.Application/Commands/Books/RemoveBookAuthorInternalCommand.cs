using System;
using BookLovers.Base.Infrastructure.Commands;

namespace BookLovers.Publication.Application.Commands.Books
{
    internal class RemoveBookAuthorInternalCommand : IInternalCommand, ICommand
    {
        public Guid Guid { get; private set; }

        public Guid AuthorGuid { get; private set; }

        public Guid BookGuid { get; private set; }

        private RemoveBookAuthorInternalCommand()
        {
        }

        public RemoveBookAuthorInternalCommand(Guid authorGuid, Guid bookGuid)
        {
            this.Guid = Guid.NewGuid();
            this.AuthorGuid = authorGuid;
            this.BookGuid = bookGuid;
        }
    }
}