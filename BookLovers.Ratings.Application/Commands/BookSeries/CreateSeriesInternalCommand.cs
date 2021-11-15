using System;
using BookLovers.Base.Infrastructure.Commands;

namespace BookLovers.Ratings.Application.Commands.BookSeries
{
    internal class CreateSeriesInternalCommand : IInternalCommand, ICommand
    {
        public Guid Guid { get; private set; }

        public Guid SeriesGuid { get; private set; }

        public int SeriesId { get; private set; }

        private CreateSeriesInternalCommand()
        {
        }

        public CreateSeriesInternalCommand(Guid seriesGuid, int seriesId)
        {
            this.Guid = Guid.NewGuid();
            this.SeriesGuid = seriesGuid;
            this.SeriesId = seriesId;
        }
    }
}