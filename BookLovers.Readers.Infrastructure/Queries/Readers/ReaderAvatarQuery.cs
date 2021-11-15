using System;
using BookLovers.Base.Infrastructure.Queries;

namespace BookLovers.Readers.Infrastructure.Queries.Readers
{
    public class ReaderAvatarQuery : IQuery<Tuple<string, string>>
    {
        public int ReaderId { get; }

        public ReaderAvatarQuery(int readerId)
        {
            this.ReaderId = readerId;
        }
    }
}