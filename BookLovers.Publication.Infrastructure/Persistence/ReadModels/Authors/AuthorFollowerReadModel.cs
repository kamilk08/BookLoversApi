namespace BookLovers.Publication.Infrastructure.Persistence.ReadModels.Authors
{
    public class AuthorFollowerReadModel
    {
        public int Id { get; set; }

        public AuthorReadModel Author { get; set; }

        public int AuthorId { get; set; }

        public ReaderReadModel FollowedBy { get; set; }

        public int FollowedById { get; set; }
    }
}