﻿using DisciplinarySystem.Domain.FinalVotes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DisciplinarySystem.Persistence.Mappings.FinalVotes
{
    public class FinalVoteDocumentMappings : IEntityTypeConfiguration<FinalVoteDocument>
    {
        public void Configure(EntityTypeBuilder<FinalVoteDocument> builder)
        {
            builder.ToTable("FinalVoteDocuments");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedNever();

            builder.Property(x => x.Name);

            builder.OwnsOne(p => p.File, p =>
            {
                p.Property(x => x.Name).HasColumnName("File").HasMaxLength(DEFAULT_NAME_LENGTH);
            });

            builder.Property(x => x.SendTime).HasDefaultValueSql("GETDATE()");

            builder.HasOne(x => x.FinalVote)
                .WithMany(x => x.Documents)
                .HasForeignKey(x => x.FinalVoteId);
        }
    }
}
