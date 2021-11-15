using System;

namespace BookLovers.Readers.Application.WriteModels.Reviews
{
    public class ReviewDetailsWriteModel
    {
        public string Content { get; set; }

        public DateTime ReviewDate { get; set; }

        public DateTime EditDate { get; set; }

        public bool MarkedAsSpoiler { get; set; }
    }
}