using System;
using BookLovers.Base.Infrastructure.Commands;

namespace BookLovers.Ratings.Application.Commands.Publishers
{
    internal class RecalculatePublisherAverageInternalCommand : IInternalCommand, ICommand
    {
        public Guid Guid { get; private set; }

        public Guid BookGuid { get; private set; }

        private RecalculatePublisherAverageInternalCommand()
        {
        }

        public RecalculatePublisherAverageInternalCommand(Guid bookGuid)
        {
            this.Guid = Guid.NewGuid();
            this.BookGuid = bookGuid;
        }
    }
}