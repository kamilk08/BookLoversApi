using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Readers.Infrastructure.Dtos.Readers;

namespace BookLovers.Readers.Infrastructure.Queries.Readers.Profiles
{
    public class ReaderProfilePrivacyQuery : IQuery<ProfilePrivacyDto>
    {
        public int ReaderId { get; }

        public ReaderProfilePrivacyQuery(int readerId)
        {
            this.ReaderId = readerId;
        }
    }
}