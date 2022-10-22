using DisciplinarySystem.Domain.FinalVotes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DisciplinarySystem.Persistence.Mappings.FinalVotes
{
    public class FinalVoteMappings : IEntityTypeConfiguration<FinalVote>
    {
        public void Configure(EntityTypeBuilder<FinalVote> builder)
        {
            builder.HasKey(b => b.Id);
            builder.Property(b => b.Id).ValueGeneratedNever();

            builder.Property(b => b.Description);
            builder.Property(b => b.CreateTime).HasDefaultValueSql("GETDATE()");

            builder.HasOne(b => b.Verdict)
                .WithMany(b => b.FinalVotes)
                .HasForeignKey(b => b.VerdictId);

            builder.HasOne(b => b.Violation)
                .WithOne(b => b.FinalVote)
                .HasForeignKey<FinalVote>(b => b.ViolationId);
        }
    }
}
