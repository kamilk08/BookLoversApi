using System;

namespace BookLovers.Publication.Application.WriteModels.PublisherCycles
{
    public class AddCycleWriteModel
    {
        public int PublisherCycleId { get; set; }

        public Guid PublisherGuid { get; set; }

        public Guid CycleGuid { get; set; }

        public string Cycle { get; set; }
    }
}