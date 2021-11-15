using System;
using BookLovers.Base.Infrastructure.Commands;

namespace BookLovers.Ratings.Application.Commands.BookSeries
{
    internal class AddSeriesBookInternalCommand : IInternalCommand, ICommand
    {
        public Guid SeriesGuid { get; private set; }

        public Guid BookGuid { get; private set; }

        public Guid Guid { get; private set; }

        private AddSeriesBookInternalCommand()
        {
        }

        public AddSeriesBookInternalCommand(Guid seriesGuid, Guid bookGuid)
        {
            this.Guid = Guid.NewGuid();
            this.SeriesGuid = seriesGuid;
            this.BookGuid = bookGuid;
        }
    }
}