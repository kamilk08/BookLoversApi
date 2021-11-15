using System;

namespace BookLovers.Ratings.Infrastructure.Dtos
{
    public class SeriesDto
    {
        public int Id { get; set; }

        public Guid SeriesGuid { get; set; }

        public int SeriesId { get; set; }
    }
}