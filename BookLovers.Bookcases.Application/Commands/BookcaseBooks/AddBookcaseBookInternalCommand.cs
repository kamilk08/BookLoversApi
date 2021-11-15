using System;
using BookLovers.Base.Infrastructure.Commands;

namespace BookLovers.Bookcases.Application.Commands.BookcaseBooks
{
    internal class AddBookcaseBookInternalCommand : ICommand, IInternalCommand
    {
        public Guid Guid { get; private set; }

        public Guid BookGuid { get; private set; }

        public int BookId { get; private set; }

        private AddBookcaseBookInternalCommand()
        {
        }

        public AddBookcaseBookInternalCommand(Guid bookGuid, int bookId)
        {
            Guid = Guid.NewGuid();
            BookGuid = bookGuid;
            BookId = bookId;
        }
    }
}