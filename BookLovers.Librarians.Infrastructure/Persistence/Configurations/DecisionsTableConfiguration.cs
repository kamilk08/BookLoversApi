using BookLovers.Librarians.Infrastructure.Persistence.ReadModels;
using System.Data.Entity.ModelConfiguration;

namespace BookLovers.Librarians.Infrastructure.Persistence.Configurations
{
    public class DecisionsTableConfiguration : EntityTypeConfiguration<DecisionReadModel>
    {
        public DecisionsTableConfiguration()
        {
            this.ToTable("Decisions");

            this.HasKey(p => p.Id);

            this.Property(p => p.Name).IsRequired();

            this.Property(p => p.Value).IsRequired();
        }
    }
}