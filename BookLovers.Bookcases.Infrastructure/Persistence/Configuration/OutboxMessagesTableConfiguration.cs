﻿using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using BookLovers.Base.Infrastructure.Messages;

namespace BookLovers.Bookcases.Infrastructure.Persistence.Configuration
{
    internal class OutboxMessagesTableConfiguration : EntityTypeConfiguration<OutboxMessage>
    {
        public OutboxMessagesTableConfiguration()
        {
            ToTable("OutboxMessages");

            HasKey(p => p.Guid);

            Property(p => p.Guid).HasColumnOrder(1)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None)
                .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("IX_Guid")
                {
                    IsUnique = true
                }));

            Property(p => p.OccuredAt).IsRequired()
                .HasColumnOrder(2);

            Property(p => p.ProcessedAt).IsOptional()
                .HasColumnOrder(3);

            Property(p => p.Type).IsRequired()
                .HasColumnOrder(4);

            Property(p => p.Assembly).IsRequired()
                .HasColumnOrder(5);

            Property(p => p.Data).IsRequired()
                .HasColumnOrder(6);
        }
    }
}