using System;
using BookLovers.Base.Infrastructure.Commands;

namespace BookLovers.Publication.Application.Commands.PublisherCycles
{
    public class RemovePublisherCycleBookCommand : ICommand
    {
        public Guid BookGuid { get; }

        public Guid CycleGuid { get; }

        private RemovePublisherCycleBookCommand()
        {
        }

        public RemovePublisherCycleBookCommand(Guid bookGuid, Guid cycleGuid)
        {
            this.BookGuid = bookGuid;
            this.CycleGuid = cycleGuid;
        }
    }
}