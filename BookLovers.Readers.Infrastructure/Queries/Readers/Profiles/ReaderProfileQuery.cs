using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Readers.Infrastructure.Dtos.Readers;

namespace BookLovers.Readers.Infrastructure.Queries.Readers.Profiles
{
    public class ReaderProfileQuery : IQuery<ProfileDto>
    {
        public int ReaderId { get; set; }

        public ReaderProfileQuery()
        {
        }

        public ReaderProfileQuery(int readerId)
        {
            this.ReaderId = readerId;
        }
    }
}