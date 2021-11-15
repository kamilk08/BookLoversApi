using System;
using System.Collections.Generic;
using BookLovers.Base.Infrastructure.Queries;

namespace BookLovers.Readers.Infrastructure.Persistence.ReadModels
{
    public class ReviewReadModel : IReadModel<ReviewReadModel>, IReadModel
    {
        public int Id { get; set; }

        public Guid Guid { get; set; }

        public int Status { get; set; }

        public BookReadModel Book { get; set; }

        public int BookId { get; set; }

        public ReaderReadModel Reader { get; set; }

        public int ReaderId { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime? EditedDate { get; set; }

        public string Review { get; set; }

        public bool MarkedAsSpoilerByReader { get; set; }

        public bool MarkedAsSpoilerByOthers { get; set; }

        public int SpoilerTagsCount { get; set; }

        public int LikesCount { get; set; }

        public int ReportsCount { get; set; }

        public IList<ReviewLikeReadModel> Likes { get; set; }

        public IList<ReviewSpoilerTagReadModel> SpoilerTags { get; set; }

        public IList<ReviewReportReadModel> ReviewReports { get; set; }

        public ReviewReadModel()
        {
            this.Likes = new List<ReviewLikeReadModel>();
            this.SpoilerTags = new List<ReviewSpoilerTagReadModel>();
            this.ReviewReports = new List<ReviewReportReadModel>();
        }
    }
}