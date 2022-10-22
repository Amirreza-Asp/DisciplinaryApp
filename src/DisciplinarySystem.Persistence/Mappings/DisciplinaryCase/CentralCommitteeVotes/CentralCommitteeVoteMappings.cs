using DisciplinarySystem.Domain.DisciplinaryCase.CentralCommitteeVotes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DisciplinarySystem.Persistence.Mappings.DisciplinaryCase.CentralCommitteeVotes
{
    public class CentralCommitteeVoteMappings : IEntityTypeConfiguration<CentralCommitteeVote>
    {
        public void Configure(EntityTypeBuilder<CentralCommitteeVote> builder)
        {
            builder.HasKey(b => b.Id);
            builder.Property(b => b.Id).ValueGeneratedNever();

            builder.Property(b => b.Description);
            builder.Property(b => b.CreateTime).HasDefaultValueSql("GETDATE()");

            builder.HasOne(b => b.Verdict)
                .WithMany(b => b.CentralCommitteeVotes)
                .HasForeignKey(b => b.VerdictId);

            builder.HasOne(b => b.Violation)
                .WithOne(b => b.CentralCommitteeVote)
                .HasForeignKey<CentralCommitteeVote>(b => b.ViolationId);
        }
    }
}
