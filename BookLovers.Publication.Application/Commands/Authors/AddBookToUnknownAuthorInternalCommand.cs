using System;
using BookLovers.Base.Infrastructure.Commands;

namespace BookLovers.Publication.Application.Commands.Authors
{
    internal class AddBookToUnknownAuthorInternalCommand : IInternalCommand, ICommand
    {
        public Guid Guid { get; }

        public Guid BookGuid { get; }

        private AddBookToUnknownAuthorInternalCommand()
        {
        }

        public AddBookToUnknownAuthorInternalCommand(Guid bookGuid)
        {
            this.Guid = Guid.NewGuid();
            this.BookGuid = bookGuid;
        }
    }
}