using BookLovers.Librarians.Infrastructure.Persistence.ReadModels;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;

namespace BookLovers.Librarians.Infrastructure.Persistence.Configurations
{
    public class ReviewReportRegistersTableConfiguration :
        EntityTypeConfiguration<ReviewReportRegisterReadModel>
    {
        public ReviewReportRegistersTableConfiguration()
        {
            this.ToTable("ReviewReportRegisters");

            this.HasKey(p => p.Id);

            this.Property(p => p.Guid).IsRequired();

            this.Property(p => p.ReviewGuid)
                .IsRequired()
                .HasColumnAnnotation("Index", new IndexAnnotation(
                    new IndexAttribute("IX_ReviewGuid")
                    {
                        IsUnique = true
                    }));

            this.Property(p => p.LibrarianGuid).IsOptional();

            this.HasMany(p => p.Reports);
        }
    }
}