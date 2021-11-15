using System;
using System.Collections.Generic;
using BookLovers.Base.Infrastructure.Queries;

namespace BookLovers.Bookcases.Infrastructure.Persistence.ReadModels
{
    public class ShelfRecordTrackerReadModel : IReadModel<ShelfRecordTrackerReadModel>
    {
        public int Id { get; set; }

        public Guid ShelfRecordTrackerGuid { get; set; }

        public Guid BookcaseGuid { get; set; }

        public int Status { get; set; }

        public List<ShelfRecordReadModel> ShelfRecords { get; set; }

        public ShelfRecordTrackerReadModel()
        {
            ShelfRecords = new List<ShelfRecordReadModel>();
        }
    }
}