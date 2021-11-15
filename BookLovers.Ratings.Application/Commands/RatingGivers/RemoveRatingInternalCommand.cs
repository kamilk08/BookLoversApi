using System;
using BookLovers.Base.Infrastructure.Commands;

namespace BookLovers.Ratings.Application.Commands.RatingGivers
{
    internal class RemoveRatingInternalCommand : IInternalCommand, ICommand
    {
        public Guid Guid { get; private set; }

        public Guid ReaderGuid { get; private set; }

        public Guid BookGuid { get; private set; }

        private RemoveRatingInternalCommand()
        {
        }

        public RemoveRatingInternalCommand(Guid readerGuid, Guid bookGuid)
        {
            this.Guid = Guid.NewGuid();
            this.ReaderGuid = readerGuid;
            this.BookGuid = bookGuid;
        }
    }
}