using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Readers.Infrastructure.Dtos.Readers;

namespace BookLovers.Readers.Infrastructure.Queries.Readers
{
    public class ReaderByUserNameQuery : IQuery<ReaderDto>
    {
        public string UserName { get; set; }

        public ReaderByUserNameQuery()
        {
        }

        public ReaderByUserNameQuery(string userName)
        {
            this.UserName = userName;
        }
    }
}