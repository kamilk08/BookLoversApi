using System;
using BookLovers.Base.Infrastructure.Commands;

namespace BookLovers.Publication.Application.Commands.Series
{
    public class ArchiveSeriesCommand : ICommand
    {
        public Guid SeriesGuid { get; }

        public ArchiveSeriesCommand(Guid seriesGuid)
        {
            this.SeriesGuid = seriesGuid;
        }
    }
}