using DisciplinarySystem.Domain.DisciplinaryCase.CentralCommitteeVotes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DisciplinarySystem.Persistence.Mappings.DisciplinaryCase.CentralCommitteeVotes
{
    public class CentralCommitteeVoteDocumentMappings : IEntityTypeConfiguration<CentralCommitteeVoteDocument>
    {
        public void Configure(EntityTypeBuilder<CentralCommitteeVoteDocument> builder)
        {
            builder.ToTable("CentralCommitteeVoteDocuments");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedNever();

            builder.Property(x => x.Name);

            builder.OwnsOne(p => p.File, p =>
            {
                p.Property(x => x.Name).HasColumnName("File").HasMaxLength(DEFAULT_NAME_LENGTH);
            });

            builder.Property(x => x.SendTime).HasDefaultValueSql("GETDATE()");

            builder.HasOne(x => x.CentralCommitteeVote)
                .WithMany(x => x.Documents)
                .HasForeignKey(x => x.CentralCommitteeVoteId);
        }
    }
}
