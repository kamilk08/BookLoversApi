using System;
using BookLovers.Base.Infrastructure.Queries;

namespace BookLovers.Bookcases.Infrastructure.Persistence.ReadModels
{
    public class SettingsManagerReadModel :
        IReadModel<SettingsManagerReadModel>
    {
        public int Id { get; set; }

        public Guid Guid { get; set; }

        public Guid BookcaseGuid { get; set; }

        public int BookcaseId { get; set; }

        public int Privacy { get; set; }

        public int Capacity { get; set; }

        public int Status { get; set; }
    }
}