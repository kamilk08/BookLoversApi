using System;

namespace BookLovers.Publication.Application.WriteModels.PublisherCycles
{
    public class RemoveCycleBookWriteModel
    {
        public Guid BookGuid { get; set; }

        public Guid CycleGuid { get; set; }
    }
}