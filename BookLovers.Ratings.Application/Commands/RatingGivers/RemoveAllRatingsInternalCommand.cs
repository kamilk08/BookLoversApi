using System;
using System.Collections.Generic;
using BookLovers.Base.Infrastructure.Commands;

namespace BookLovers.Ratings.Application.Commands.RatingGivers
{
    internal class RemoveAllRatingsInternalCommand : IInternalCommand, ICommand
    {
        public Guid Guid { get; private set; }

        public Guid ReaderGuid { get; private set; }

        public int ReaderId { get; private set; }

        public List<int> BookIds { get; private set; }

        private RemoveAllRatingsInternalCommand()
        {
        }

        public RemoveAllRatingsInternalCommand(Guid readerGuid, int readerId, List<int> bookIds)
        {
            this.Guid = Guid.NewGuid();
            this.ReaderGuid = readerGuid;
            this.ReaderId = readerId;
            this.BookIds = bookIds;
        }
    }
}