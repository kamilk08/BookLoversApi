using System;
using BookLovers.Base.Infrastructure.Commands;

namespace BookLovers.Publication.Application.Commands.PublisherCycles
{
    public class AddPublisherCycleBookCommand : ICommand
    {
        public Guid CycleGuid { get; }

        public Guid BookGuid { get; }

        public AddPublisherCycleBookCommand(Guid cycleGuid, Guid bookGuid)
        {
            this.CycleGuid = cycleGuid;
            this.BookGuid = bookGuid;
        }
    }
}