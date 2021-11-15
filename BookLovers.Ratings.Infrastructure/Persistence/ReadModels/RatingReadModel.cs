namespace BookLovers.Ratings.Infrastructure.Persistence.ReadModels
{
    public class RatingReadModel
    {
        public int Id { get; set; }

        public int BookId { get; set; }

        public int ReaderId { get; set; }

        public int Stars { get; set; }
    }
}