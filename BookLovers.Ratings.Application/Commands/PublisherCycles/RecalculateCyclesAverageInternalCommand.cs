using System;
using BookLovers.Base.Infrastructure.Commands;

namespace BookLovers.Ratings.Application.Commands.PublisherCycles
{
    internal class RecalculateCyclesAverageInternalCommand : IInternalCommand, ICommand
    {
        public Guid Guid { get; private set; }

        public Guid CycleGuid { get; private set; }

        private RecalculateCyclesAverageInternalCommand()
        {
        }

        public RecalculateCyclesAverageInternalCommand(Guid cycleGuid)
        {
            this.Guid = Guid.NewGuid();
            this.CycleGuid = cycleGuid;
        }
    }
}