using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using BookLovers.Bookcases.Infrastructure.Persistence.ReadModels;

namespace BookLovers.Bookcases.Infrastructure.Persistence.Configuration
{
    internal class ShelfRecordsConfiguration : EntityTypeConfiguration<ShelfRecordReadModel>
    {
        public ShelfRecordsConfiguration()
        {
            ToTable("ShelfRecords");

            Property(p => p.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(p => p.BookId)
                .HasColumnName("BookRowId");
        }
    }
}