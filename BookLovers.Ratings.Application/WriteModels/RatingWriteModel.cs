using System;

namespace BookLovers.Ratings.Application.WriteModels
{
    public class RatingWriteModel
    {
        public Guid Id { get; set; }

        public Guid ReaderId { get; set; }

        public byte Star { get; set; }
    }
}