using System;

namespace BookLovers.Publication.Application.WriteModels.PublisherCycles
{
    public class AddCycleBookWriteModel
    {
        public Guid CycleGuid { get; set; }

        public Guid BookGuid { get; set; }
    }
}