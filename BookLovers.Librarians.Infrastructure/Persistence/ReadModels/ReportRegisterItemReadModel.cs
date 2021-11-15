using System;

namespace BookLovers.Librarians.Infrastructure.Persistence.ReadModels
{
    public class ReportRegisterItemReadModel
    {
        public int Id { get; set; }

        public Guid ReportedByGuid { get; set; }

        public int ReportReasonId { get; set; }

        public string ReportReasonName { get; set; }
    }
}