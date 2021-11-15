using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using BookLovers.Readers.Infrastructure.Persistence.ReadModels;

namespace BookLovers.Readers.Infrastructure.Persistence.Configuration
{
    internal class SexesTableConfiguration : EntityTypeConfiguration<SexReadModel>
    {
        public SexesTableConfiguration()
        {
            this.ToTable("Sexes");

            this.Property(p => p.Name).IsRequired();

            this.Property(p => p.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
        }
    }
}