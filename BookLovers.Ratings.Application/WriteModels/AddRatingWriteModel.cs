using System;

namespace BookLovers.Ratings.Application.WriteModels
{
    public class AddRatingWriteModel
    {
        public Guid BookGuid { get; set; }

        public int Stars { get; set; }

        public int RatingId { get; set; }
    }
}