using System.Data.Entity.ModelConfiguration;
using BookLovers.Readers.Infrastructure.Persistence.ReadModels;

namespace BookLovers.Readers.Infrastructure.Persistence.Configuration
{
    public class AddedResourcesTableConfiguration : EntityTypeConfiguration<AddedResourceReadModel>
    {
        public AddedResourcesTableConfiguration()
        {
            this.ToTable("AddedResources");

            this.HasKey(p => p.Id);

            this.Property(p => p.Id).HasColumnOrder(1);

            this.Property(p => p.ResourceGuid).HasColumnOrder(2).IsRequired();
        }
    }
}