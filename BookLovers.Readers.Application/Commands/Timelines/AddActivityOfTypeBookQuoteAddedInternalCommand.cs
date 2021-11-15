using System;
using BookLovers.Base.Infrastructure.Commands;

namespace BookLovers.Readers.Application.Commands.Timelines
{
    internal class AddActivityOfTypeBookQuoteAddedInternalCommand : IInternalCommand, ICommand
    {
        public Guid Guid { get; private set; }

        public Guid QuoteGuid { get; private set; }

        public Guid ReaderGuid { get; private set; }

        public AddActivityOfTypeBookQuoteAddedInternalCommand()
        {
        }

        public AddActivityOfTypeBookQuoteAddedInternalCommand(Guid quoteGuid, Guid readerGuid)
        {
            Guid = Guid.NewGuid();
            QuoteGuid = quoteGuid;
            ReaderGuid = readerGuid;
        }
    }
}