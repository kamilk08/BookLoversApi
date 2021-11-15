using System;
using BookLovers.Base.Infrastructure.Commands;

namespace BookLovers.Ratings.Application.Commands.PublisherCycles
{
    internal class RemovePublisherCycleBookInternalCommand : IInternalCommand, ICommand
    {
        public Guid Guid { get; private set; }

        public Guid PublisherCycleGuid { get; private set; }

        public Guid BookGuid { get; private set; }

        public RemovePublisherCycleBookInternalCommand()
        {
        }

        public RemovePublisherCycleBookInternalCommand(Guid publisherCycleGuid, Guid bookGuid)
        {
            this.Guid = Guid.NewGuid();
            this.PublisherCycleGuid = publisherCycleGuid;
            this.BookGuid = bookGuid;
        }
    }
}