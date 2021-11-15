using System;
using System.Collections.Generic;

namespace BookLovers.Readers.Infrastructure.Dtos.Reviews
{
    public class ReviewDto
    {
        public int Id { get; set; }

        public Guid Guid { get; set; }

        public string Review { get; set; }

        public byte ReviewStatus { get; set; }

        public Guid BookGuid { get; set; }

        public int BookId { get; set; }

        public Guid ReaderGuid { get; set; }

        public int ReaderId { get; set; }

        public string AddedBy { get; set; }

        public DateTime ReviewDate { get; set; }

        public DateTime? EditedDate { get; set; }

        public bool MarkedAsSpoiler { get; set; }

        public bool MarkedAsSpoilerByOthers { get; set; }

        public List<ReviewLikeDto> Likes { get; set; }

        public List<SpoilerTagDto> SpoilerTags { get; set; }

        public List<ReviewReportDto> ReviewReports { get; set; }
    }
}