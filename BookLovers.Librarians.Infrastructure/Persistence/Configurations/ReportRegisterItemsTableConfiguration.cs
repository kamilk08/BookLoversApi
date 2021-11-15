using BookLovers.Librarians.Infrastructure.Persistence.ReadModels;
using System.Data.Entity.ModelConfiguration;

namespace BookLovers.Librarians.Infrastructure.Persistence.Configurations
{
    internal class ReportRegisterItemsTableConfiguration :
        EntityTypeConfiguration<ReportRegisterItemReadModel>
    {
        public ReportRegisterItemsTableConfiguration()
        {
            this.ToTable("ReviewReportItems");
        }
    }
}