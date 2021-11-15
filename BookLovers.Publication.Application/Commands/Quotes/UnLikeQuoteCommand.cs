using System;
using BookLovers.Base.Infrastructure.Commands;

namespace BookLovers.Publication.Application.Commands.Quotes
{
    public class UnLikeQuoteCommand : ICommand
    {
        public Guid QuoteGuid { get; }

        public UnLikeQuoteCommand(Guid quoteGuid)
        {
            this.QuoteGuid = quoteGuid;
        }
    }
}