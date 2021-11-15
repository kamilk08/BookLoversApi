using System;
using BookLovers.Base.Infrastructure.Queries;

namespace BookLovers.Readers.Infrastructure.Queries.Readers.Profiles
{
    public class DoesProfileBelongToReaderQuery : IQuery<bool>
    {
        public Guid ReaderGuid { get; }

        public Guid ProfileGuid { get; }

        public DoesProfileBelongToReaderQuery(Guid readerGuid, Guid profileGuid)
        {
            this.ReaderGuid = readerGuid;
            this.ProfileGuid = profileGuid;
        }
    }
}