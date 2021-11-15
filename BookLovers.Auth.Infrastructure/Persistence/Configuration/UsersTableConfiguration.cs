using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using BookLovers.Auth.Infrastructure.Persistence.ReadModels;

namespace BookLovers.Auth.Infrastructure.Persistence.Configuration
{
    internal class UsersTableConfiguration : EntityTypeConfiguration<UserReadModel>
    {
        public UsersTableConfiguration()
        {
            ToTable("Users");

            HasKey(p => p.Id);

            Property(p => p.Id).HasColumnOrder(1);

            Property(p => p.Guid).IsRequired()
                .HasColumnName("ReaderGuid")
                .HasColumnOrder(2);

            Property(p => p.UserName)
                .HasColumnOrder(3)
                .HasMaxLength(byte.MaxValue)
                .HasColumnAnnotation(
                    "Index",
                    new IndexAnnotation(new IndexAttribute("IX_UserName")
                    {
                        IsUnique = true
                    }));

            HasRequired(p => p.Account);

            HasMany(p => p.Roles).WithMany()
                .Map(s => s.ToTable("UserRoles")
                    .MapLeftKey("UserId")
                    .MapRightKey("RoleId"));
        }
    }
}