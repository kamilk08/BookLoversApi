using System;

namespace BookLovers.Readers.Infrastructure.Dtos.Readers
{
    public class TimeLineDto
    {
        public int Id { get; set; }

        public Guid Guid { get; set; }

        public int ReaderId { get; set; }

        public int ActivitiesCount { get; set; }
    }
}