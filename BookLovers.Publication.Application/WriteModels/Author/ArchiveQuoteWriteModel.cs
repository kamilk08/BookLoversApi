using System;

namespace BookLovers.Publication.Application.WriteModels.Author
{
    public class ArchiveQuoteWriteModel
    {
        public Guid QuoteGuid { get; set; }

        public Guid ReaderGuid { get; set; }
    }
}