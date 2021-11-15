using System.Data.Entity.ModelConfiguration;
using BookLovers.Readers.Infrastructure.Persistence.ReadModels;

namespace BookLovers.Readers.Infrastructure.Persistence.Configuration
{
    internal class ReviewsTableConfiguration : EntityTypeConfiguration<ReviewReadModel>
    {
        public ReviewsTableConfiguration()
        {
            this.ToTable("Reviews");

            this.HasKey(p => p.Id);

            this.Property(p => p.Id).HasColumnOrder(1);

            this.Property(p => p.Guid).IsRequired().HasColumnName("Guid").HasColumnOrder(2);

            this.Property(p => p.Review).IsRequired().HasColumnOrder(3);

            this.Property(p => p.CreateDate).IsRequired()
                .HasColumnName("Date").HasColumnType("datetime2")
                .HasPrecision(0).HasColumnOrder(4);

            this.Property(p => p.EditedDate).IsOptional()
                .HasColumnName("EditedDate").HasColumnType("datetime2")
                .HasPrecision(0).HasColumnOrder(5);

            this.Property(p => p.LikesCount).IsRequired()
                .HasColumnName("LikesCount").HasColumnOrder(6);

            this.Property(p => p.Status).IsRequired().HasColumnOrder(7);

            this.HasRequired(p => p.Reader);
        }
    }
}