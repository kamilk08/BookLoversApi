using System;
using BookLovers.Base.Infrastructure.Commands;

namespace BookLovers.Ratings.Application.Commands.RatingGivers
{
    internal class ArchiveRatingGiverInternalCommand : IInternalCommand, ICommand
    {
        public Guid Guid { get; private set; }

        public Guid ReaderGuid { get; private set; }

        private ArchiveRatingGiverInternalCommand()
        {
        }

        public ArchiveRatingGiverInternalCommand(Guid readerGuid)
        {
            this.Guid = Guid.NewGuid();
            this.ReaderGuid = readerGuid;
        }
    }
}