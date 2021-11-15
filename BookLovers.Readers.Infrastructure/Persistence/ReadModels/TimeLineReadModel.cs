using System;
using System.Collections.Generic;
using BookLovers.Base.Infrastructure.Queries;

namespace BookLovers.Readers.Infrastructure.Persistence.ReadModels
{
    public class TimeLineReadModel : IReadModel<TimeLineReadModel>, IReadModel
    {
        public int Id { get; set; }

        public Guid Guid { get; set; }

        public int Status { get; set; }

        public int ReaderId { get; set; }

        public IList<TimeLineActivityReadModel> Actvities { get; set; }

        public TimeLineReadModel()
        {
            this.Actvities = new List<TimeLineActivityReadModel>();
        }
    }
}