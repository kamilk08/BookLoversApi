using System;
using System.Collections.Generic;

namespace BookLovers.Seed.Models
{
    public class SeedBook
    {
        public Guid BookGuid { get; set; }

        public string Isbn { get; set; }

        public string Title { get; set; }

        public DateTime? Published { get; set; }

        public string PublisherName { get; set; }

        public string SeriesName { get; set; }

        public int PositionInSeries { get; set; }

        public int Pages { get; set; }

        public List<string> Authors { get; set; }

        public string Description { get; set; }
    }
}