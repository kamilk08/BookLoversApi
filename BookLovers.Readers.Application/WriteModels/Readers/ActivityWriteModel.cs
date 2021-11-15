using System;

namespace BookLovers.Readers.Application.WriteModels.Readers
{
    public class ActivityWriteModel
    {
        public Guid TimeLineObjectId { get; set; }

        public int ActivityType { get; set; }

        public string Title { get; set; }

        public DateTime Date { get; set; }

        public bool ShowToOthers { get; set; }
    }
}