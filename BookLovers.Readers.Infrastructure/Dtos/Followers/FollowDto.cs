using BookLovers.Readers.Infrastructure.Dtos.Readers;

namespace BookLovers.Readers.Infrastructure.Dtos.Followers
{
    public class FollowDto
    {
        public int Id { get; set; }

        public ReaderDto Reader { get; set; }
    }
}