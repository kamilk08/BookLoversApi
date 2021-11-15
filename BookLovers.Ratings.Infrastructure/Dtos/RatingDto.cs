namespace BookLovers.Ratings.Infrastructure.Dtos
{
    public class RatingDto
    {
        public int Id { get; set; }

        public int BookId { get; set; }

        public int ReaderId { get; set; }

        public int Stars { get; set; }
    }
}