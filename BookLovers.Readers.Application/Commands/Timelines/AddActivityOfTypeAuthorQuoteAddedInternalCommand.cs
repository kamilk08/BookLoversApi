using System;
using BookLovers.Base.Infrastructure.Commands;

namespace BookLovers.Readers.Application.Commands.Timelines
{
    internal class AddActivityOfTypeAuthorQuoteAddedInternalCommand : IInternalCommand, ICommand
    {
        public Guid Guid { get; private set; }

        public Guid ReaderGuid { get; private set; }

        public Guid QuoteGuid { get; private set; }

        private AddActivityOfTypeAuthorQuoteAddedInternalCommand()
        {
        }

        public AddActivityOfTypeAuthorQuoteAddedInternalCommand(Guid readerGuid, Guid quoteGuid)
        {
            Guid = Guid.NewGuid();
            ReaderGuid = readerGuid;
            QuoteGuid = quoteGuid;
        }
    }
}