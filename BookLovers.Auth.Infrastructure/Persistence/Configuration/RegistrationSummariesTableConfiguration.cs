using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using BookLovers.Auth.Infrastructure.Persistence.ReadModels;

namespace BookLovers.Auth.Infrastructure.Persistence.Configuration
{
    internal class RegistrationSummariesTableConfiguration :
        EntityTypeConfiguration<RegistrationSummaryReadModel>
    {
        public RegistrationSummariesTableConfiguration()
        {
            ToTable("RegistrationSummaries");

            HasKey(p => p.Id);

            Property(p => p.Guid).IsRequired();

            Property(p => p.UserGuid).IsRequired();

            Property(p => p.CreateAt).IsRequired();

            Property(p => p.ExpiredAt).IsRequired();

            Property(p => p.Email).IsRequired()
                .HasMaxLength(255)
                .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("IX_Email")
                {
                    IsUnique = true
                }));

            Property(p => p.CompletedAt).IsOptional();
        }
    }
}