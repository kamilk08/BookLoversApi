using System.Data.Entity.ModelConfiguration;
using BookLovers.Publication.Infrastructure.Persistence.ReadModels.Authors;

namespace BookLovers.Publication.Infrastructure.Persistence.Configuration.Authors
{
    internal class AuthorFollowersTableConfiguration : EntityTypeConfiguration<AuthorFollowerReadModel>
    {
        public AuthorFollowersTableConfiguration()
        {
            this.ToTable("AuthorFollowers");

            this.HasKey(p => p.Id);

            this.Property(p => p.Id).HasColumnOrder(1);

            this.HasRequired(p => p.Author).WithMany(p => p.AuthorFollowers);

            this.HasRequired(p => p.FollowedBy).WithMany()
                .WillCascadeOnDelete(true);
        }
    }
}