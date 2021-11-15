using System;
using BookLovers.Base.Infrastructure.Commands;

namespace BookLovers.Ratings.Application.Commands.BookSeries
{
    internal class RecalculateSeriesAverageInternalCommand : IInternalCommand, ICommand
    {
        public Guid Guid { get; private set; }

        public Guid BookGuid { get; private set; }

        private RecalculateSeriesAverageInternalCommand()
        {
        }

        public RecalculateSeriesAverageInternalCommand(Guid bookGuid)
        {
            this.Guid = Guid.NewGuid();
            this.BookGuid = bookGuid;
        }
    }
}