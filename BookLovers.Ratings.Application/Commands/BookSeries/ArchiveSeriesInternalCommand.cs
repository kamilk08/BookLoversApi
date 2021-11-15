using System;
using BookLovers.Base.Infrastructure.Commands;

namespace BookLovers.Ratings.Application.Commands.BookSeries
{
    internal class ArchiveSeriesInternalCommand : IInternalCommand, ICommand
    {
        public Guid Guid { get; private set; }

        public Guid SeriesGuid { get; private set; }

        private ArchiveSeriesInternalCommand()
        {
        }

        public ArchiveSeriesInternalCommand(Guid seriesGuid)
        {
            this.Guid = Guid.NewGuid();
            this.SeriesGuid = seriesGuid;
        }
    }
}