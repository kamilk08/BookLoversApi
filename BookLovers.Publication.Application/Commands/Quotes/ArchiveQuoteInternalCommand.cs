using System;
using BookLovers.Base.Infrastructure.Commands;

namespace BookLovers.Publication.Application.Commands.Quotes
{
    internal class ArchiveQuoteInternalCommand : IInternalCommand, ICommand
    {
        public Guid Guid { get; private set; }

        public Guid QuoteGuid { get; private set; }

        private ArchiveQuoteInternalCommand()
        {
        }

        public ArchiveQuoteInternalCommand(Guid quoteGuid)
        {
            this.Guid = Guid.NewGuid();
            this.QuoteGuid = quoteGuid;
        }
    }
}