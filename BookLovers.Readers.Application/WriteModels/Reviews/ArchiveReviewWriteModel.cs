using System;

namespace BookLovers.Readers.Application.WriteModels.Reviews
{
    public class ArchiveReviewWriteModel
    {
        public Guid ReviewId { get; set; }

        public Guid ReaderId { get; set; }
    }
}