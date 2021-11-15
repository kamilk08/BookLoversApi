using System.Data.Entity.ModelConfiguration;
using BookLovers.Readers.Infrastructure.Persistence.ReadModels;

namespace BookLovers.Readers.Infrastructure.Persistence.Configuration
{
    internal class ReviewReportsTableConfiguration : EntityTypeConfiguration<ReviewReportReadModel>
    {
        public ReviewReportsTableConfiguration()
        {
            this.ToTable("ReviewReports");

            this.HasKey(p => p.Id);

            this.Property(p => p.Id).HasColumnOrder(1);

            this.Property(p => p.ReaderId).HasColumnOrder(2);

            this.Property(p => p.ReviewId).HasColumnOrder(3);
        }
    }
}