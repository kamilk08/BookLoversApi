using System;
using BookLovers.Base.Infrastructure.Commands;

namespace BookLovers.Publication.Application.Commands.Authors
{
    internal class AddQuoteToAuthorInternalCommand : IInternalCommand, ICommand
    {
        public Guid Guid { get; private set; }

        public Guid QuoteGuid { get; private set; }

        public Guid AuthorGuid { get; private set; }

        private AddQuoteToAuthorInternalCommand()
        {
        }

        public AddQuoteToAuthorInternalCommand(Guid quoteGuid, Guid authorGuid)
        {
            this.Guid = Guid.NewGuid();
            this.QuoteGuid = quoteGuid;
            this.AuthorGuid = authorGuid;
        }
    }
}