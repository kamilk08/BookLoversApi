namespace BookLovers.Readers.Infrastructure.Persistence.ReadModels
{
    public class ReviewReportReadModel
    {
        public int Id { get; set; }

        public ReviewReadModel Review { get; set; }

        public int ReviewId { get; set; }

        public int ReaderId { get; set; }
    }
}