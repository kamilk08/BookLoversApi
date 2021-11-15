using System;
using BookLovers.Base.Infrastructure.Commands;

namespace BookLovers.Publication.Application.Commands.Books
{
    internal class ChangeBookSeriesInternalCommand : IInternalCommand, ICommand
    {
        public Guid Guid { get; private set; }

        public Guid BookGuid { get; private set; }

        public Guid? SeriesGuid { get; private set; }

        public int? PositionInSeries { get; private set; }

        private ChangeBookSeriesInternalCommand()
        {
        }

        public ChangeBookSeriesInternalCommand(Guid bookGuid, Guid? seriesGuid, int? positionInSeries)
        {
            this.Guid = Guid.NewGuid();
            this.BookGuid = bookGuid;
            this.SeriesGuid = seriesGuid;
            this.PositionInSeries = positionInSeries;
        }
    }
}