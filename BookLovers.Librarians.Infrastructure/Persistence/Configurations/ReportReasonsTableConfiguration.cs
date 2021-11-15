using BookLovers.Librarians.Infrastructure.Persistence.ReadModels;
using System.Data.Entity.ModelConfiguration;

namespace BookLovers.Librarians.Infrastructure.Persistence.Configurations
{
    internal class ReportReasonsTableConfiguration : EntityTypeConfiguration<ReportReasonReadModel>
    {
        public ReportReasonsTableConfiguration()
        {
            this.ToTable("ReportReasons");

            this.HasKey(p => p.ReasonId);
        }
    }
}