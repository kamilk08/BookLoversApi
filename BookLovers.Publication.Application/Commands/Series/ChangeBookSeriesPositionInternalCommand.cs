using System;
using BookLovers.Base.Infrastructure.Commands;

namespace BookLovers.Publication.Application.Commands.Series
{
    internal class ChangeBookSeriesPositionInternalCommand : IInternalCommand, ICommand
    {
        public Guid Guid { get; private set; }

        public Guid SeriesGuid { get; private set; }

        public Guid BookGuid { get; private set; }

        public int Position { get; private set; }

        private ChangeBookSeriesPositionInternalCommand()
        {
        }

        public ChangeBookSeriesPositionInternalCommand(Guid seriesGuid, Guid bookGuid, int position)
        {
            this.Guid = Guid.NewGuid();
            this.SeriesGuid = seriesGuid;
            this.BookGuid = bookGuid;
            this.Position = position;
        }
    }
}