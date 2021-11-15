using System;

namespace BookLovers.Readers.Infrastructure.Dtos.Readers
{
    public class TimeLineActivityDto
    {
        public int Id { get; set; }

        public Guid ActivityObjectGuid { get; set; }

        public string Title { get; set; }

        public byte ActivityType { get; set; }

        public DateTime Date { get; set; }

        public bool Show { get; set; }
    }
}