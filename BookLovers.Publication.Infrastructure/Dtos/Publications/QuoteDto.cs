using System;
using System.Collections.Generic;

namespace BookLovers.Publication.Infrastructure.Dtos.Publications
{
    public class QuoteDto
    {
        public int Id { get; set; }

        public Guid Guid { get; set; }

        public string Quote { get; set; }

        public DateTime AddedAt { get; set; }

        public Guid QuoteFromGuid { get; set; }

        public int QuoteFromId { get; set; }

        public int QuoteType { get; set; }

        public int ReaderId { get; set; }

        public Guid ReaderGuid { get; set; }

        public IList<QuoteLikeDto> QuoteLikes { get; set; }
    }
}