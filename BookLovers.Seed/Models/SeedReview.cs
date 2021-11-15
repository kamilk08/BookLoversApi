using System;

namespace BookLovers.Seed.Models
{
    public class SeedReview
    {
        public Guid ReviewGuid { get; set; }

        public string Content { get; set; }

        public DateTime ReviewDate { get; set; }

        public bool MarkedAsSpoiler { get; set; }

        public SeedUser SeedUser { get; set; }
    }
}