using System;
using BookLovers.Base.Infrastructure.Commands;

namespace BookLovers.Publication.Application.Commands.Series
{
    internal class AddSeriesBookInternalCommand : IInternalCommand, ICommand
    {
        public Guid Guid { get; private set; }

        public Guid? SeriesGuid { get; private set; }

        public int? PositionInSeries { get; private set; }

        public Guid BookGuid { get; private set; }

        public bool SendIntegrationEvent { get; private set; }

        private AddSeriesBookInternalCommand()
        {
        }

        public AddSeriesBookInternalCommand(
            Guid? seriesGuid,
            int? positionInSeries,
            Guid bookGuid,
            bool sendIntegrationEvent = true)
        {
            this.Guid = Guid.NewGuid();
            this.SeriesGuid = seriesGuid;
            this.PositionInSeries = positionInSeries;
            this.BookGuid = bookGuid;
            this.SendIntegrationEvent = sendIntegrationEvent;
        }
    }
}