using System;
using BookLovers.Base.Infrastructure.Commands;

namespace BookLovers.Ratings.Application.Commands.RatingGivers
{
    internal class CreateRatingGiverInternalCommand : IInternalCommand, ICommand
    {
        public Guid Guid { get; private set; }

        public Guid ReaderGuid { get; private set; }

        public int ReaderId { get; private set; }

        private CreateRatingGiverInternalCommand()
        {
        }

        public CreateRatingGiverInternalCommand(Guid readerGuid, int readerId)
        {
            this.Guid = Guid.NewGuid();
            this.ReaderGuid = readerGuid;
            this.ReaderId = readerId;
        }
    }
}