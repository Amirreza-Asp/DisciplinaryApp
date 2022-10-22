using DisciplinarySystem.Domain.PrimaryVotes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DisciplinarySystem.Persistence.Mappings.PrimaryVotes
{
	public class PrimaryVoteMappings : IEntityTypeConfiguration<PrimaryVote>
	{
		public void Configure(EntityTypeBuilder<PrimaryVote> builder)
		{
			builder.HasKey(b => b.Id);
			builder.Property(b => b.Id).ValueGeneratedNever();

			builder.Property(b => b.Description);
			builder.Property(b => b.CreateTime).HasDefaultValueSql("GETDATE()");

			builder.HasOne(b => b.Verdict)
				.WithMany(b => b.PrimaryVotes)
				.HasForeignKey(b => b.VerdictId);

			builder.HasOne(b => b.Violation)
				.WithOne(b => b.PrimaryVote)
				.HasForeignKey<PrimaryVote>(b => b.ViolationId);
		}
	}
}
