using System;
using BookLovers.Base.Infrastructure.Commands;

namespace BookLovers.Publication.Application.Commands.Quotes
{
    public class LikeQuoteCommand : ICommand
    {
        public Guid QuoteGuid { get; }

        public LikeQuoteCommand(Guid quoteGuid)
        {
            this.QuoteGuid = quoteGuid;
        }
    }
}