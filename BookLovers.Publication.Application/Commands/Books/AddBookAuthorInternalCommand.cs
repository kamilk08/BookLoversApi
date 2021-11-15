using System;
using BookLovers.Base.Infrastructure.Commands;

namespace BookLovers.Publication.Application.Commands.Books
{
    internal class AddBookAuthorInternalCommand : ICommand, IInternalCommand
    {
        public Guid Guid { get; private set; }

        public Guid BookGuid { get; private set; }

        public Guid AuthorGuid { get; private set; }

        private AddBookAuthorInternalCommand()
        {
        }

        public AddBookAuthorInternalCommand(Guid bookGuid, Guid authorGuid)
        {
            this.Guid = Guid.NewGuid();
            this.BookGuid = bookGuid;
            this.AuthorGuid = authorGuid;
        }
    }
}