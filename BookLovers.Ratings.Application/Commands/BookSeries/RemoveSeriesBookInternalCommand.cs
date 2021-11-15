using System;
using BookLovers.Base.Infrastructure.Commands;

namespace BookLovers.Ratings.Application.Commands.BookSeries
{
    internal class RemoveSeriesBookInternalCommand : IInternalCommand, ICommand
    {
        public Guid Guid { get; private set; }

        public Guid SeriesGuid { get; private set; }

        public Guid BookGuid { get; private set; }

        private RemoveSeriesBookInternalCommand()
        {
        }

        public RemoveSeriesBookInternalCommand(Guid seriesGuid, Guid bookGuid)
        {
            this.Guid = Guid.NewGuid();
            this.SeriesGuid = seriesGuid;
            this.BookGuid = bookGuid;
        }
    }
}