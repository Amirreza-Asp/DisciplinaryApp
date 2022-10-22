using DisciplinarySystem.Domain.PrimaryVotes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DisciplinarySystem.Persistence.Mappings.PrimaryVotes
{
    public class PrimaryVoteDocumentMappings : IEntityTypeConfiguration<PrimaryVoteDocument>
    {
        public void Configure(EntityTypeBuilder<PrimaryVoteDocument> builder)
        {
            builder.ToTable("PrimaryVoteDocuments");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedNever();

            builder.Property(x => x.Name);

            builder.OwnsOne(p => p.File, p =>
            {
                p.Property(x => x.Name).HasColumnName("File").HasMaxLength(DEFAULT_NAME_LENGTH);
            });

            builder.Property(x => x.SendTime).HasDefaultValueSql("GETDATE()");

            builder.HasOne(x => x.PrimaryVote)
                .WithMany(x => x.Documents)
                .HasForeignKey(x => x.PrimaryVoteId);
        }
    }
}
