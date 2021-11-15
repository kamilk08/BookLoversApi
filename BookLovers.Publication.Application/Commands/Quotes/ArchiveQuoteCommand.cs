using System;
using BookLovers.Base.Infrastructure.Commands;

namespace BookLovers.Publication.Application.Commands.Quotes
{
    public class ArchiveQuoteCommand : ICommand
    {
        public Guid QuoteGuid { get; }

        public ArchiveQuoteCommand(Guid quoteGuid)
        {
            this.QuoteGuid = quoteGuid;
        }
    }
}