namespace BookLovers.Publication.Infrastructure.Dtos.Publications
{
    public class AuthorFollowerDto
    {
        public int Id { get; set; }

        public int AuthorId { get; set; }

        public int FollowedById { get; set; }
    }
}