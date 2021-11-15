using System.Data.Entity.ModelConfiguration;
using BookLovers.Readers.Infrastructure.Persistence.ReadModels;

namespace BookLovers.Readers.Infrastructure.Persistence.Configuration
{
    internal class StatisticsTableConfiguration : EntityTypeConfiguration<StatisticsReadModel>
    {
        public StatisticsTableConfiguration()
        {
            this.ToTable("Statistics");
        }
    }
}