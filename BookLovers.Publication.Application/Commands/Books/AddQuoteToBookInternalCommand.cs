using System;
using BookLovers.Base.Infrastructure.Commands;

namespace BookLovers.Publication.Application.Commands.Books
{
    internal class AddQuoteToBookInternalCommand : IInternalCommand, ICommand
    {
        public Guid Guid { get; private set; }

        public Guid QuoteGuid { get; private set; }

        public Guid BookGuid { get; private set; }

        private AddQuoteToBookInternalCommand()
        {
        }

        public AddQuoteToBookInternalCommand(Guid quoteGuid, Guid bookGuid)
        {
            this.Guid = Guid.NewGuid();
            this.QuoteGuid = quoteGuid;
            this.BookGuid = bookGuid;
        }
    }
}