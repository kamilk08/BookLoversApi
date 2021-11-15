using System;

namespace BookLovers.Readers.Application.WriteModels.Reviews
{
    public class ReviewWriteModel
    {
        public int ReviewId { get; set; }

        public Guid ReviewGuid { get; set; }

        public Guid BookGuid { get; set; }

        public ReviewDetailsWriteModel ReviewDetails { get; set; }
    }
}