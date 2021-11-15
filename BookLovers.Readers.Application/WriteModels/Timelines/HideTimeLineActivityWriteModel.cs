using System;

namespace BookLovers.Readers.Application.WriteModels.Timelines
{
    public class HideTimeLineActivityWriteModel
    {
        public Guid TimeLineObjectGuid { get; set; }

        public DateTime OccuredAt { get; set; }

        public int ActivityTypeId { get; set; }
    }
}