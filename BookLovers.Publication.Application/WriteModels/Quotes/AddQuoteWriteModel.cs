using System;

namespace BookLovers.Publication.Application.WriteModels.Quotes
{
    public class AddQuoteWriteModel
    {
        public int QuoteId { get; set; }

        public Guid QuoteGuid { get; set; }

        public Guid QuotedGuid { get; set; }

        public string Quote { get; set; }

        public DateTime AddedAt { get; set; }
    }
}