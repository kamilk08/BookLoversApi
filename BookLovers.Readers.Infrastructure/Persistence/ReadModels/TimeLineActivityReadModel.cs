using System;
using BookLovers.Base.Infrastructure.Queries;

namespace BookLovers.Readers.Infrastructure.Persistence.ReadModels
{
    public class TimeLineActivityReadModel : IReadModel
    {
        public int Id { get; set; }

        public Guid ActivityObjectGuid { get; set; }

        public string Title { get; set; }

        public int ActivityType { get; set; }

        public DateTime Date { get; set; }

        public bool Show { get; set; }
    }
}