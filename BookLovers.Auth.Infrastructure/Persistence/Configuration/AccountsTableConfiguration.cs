using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using BookLovers.Auth.Infrastructure.Persistence.ReadModels;

namespace BookLovers.Auth.Infrastructure.Persistence.Configuration
{
    internal class AccountsTableConfiguration : EntityTypeConfiguration<AccountReadModel>
    {
        public AccountsTableConfiguration()
        {
            ToTable("Accounts");
            Property(p => p.Email)
                .IsRequired()
                .HasColumnName("Email")
                .HasMaxLength(byte.MaxValue)
                .HasColumnAnnotation(
                    "Index",
                    new IndexAnnotation(new IndexAttribute("IX_Index")
                    {
                        IsUnique = true
                    }));

            Property(p => p.Hash).IsRequired();

            Property(p => p.Salt).IsRequired();

            Property(p => p.ConfirmationDate).IsOptional();

            Property(p => p.AccountCreateDate)
                .IsRequired()
                .HasColumnName("AccountCreateDate")
                .HasColumnType("datetime2").HasPrecision(0);
        }
    }
}