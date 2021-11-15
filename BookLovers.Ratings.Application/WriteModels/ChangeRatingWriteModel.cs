using System;

namespace BookLovers.Ratings.Application.WriteModels
{
    public class ChangeRatingWriteModel
    {
        public Guid BookGuid { get; set; }

        public int Stars { get; set; }
    }
}