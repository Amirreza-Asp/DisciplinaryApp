using DisciplinarySystem.Domain.Meetings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DisciplinarySystem.Persistence.Mappings.Meetings
{
	public class ProceedingsMappings : IEntityTypeConfiguration<Proceedings>
	{
		public void Configure(EntityTypeBuilder<Proceedings> builder)
		{
			builder.HasKey(x => x.Id);
			builder.Property(b => b.Id).ValueGeneratedNever();

			builder.Property(b => b.CreateDate).HasDefaultValueSql("GETDATE()");
			builder.Property(b => b.Title);
			builder.Property(b => b.Description);

			builder.HasOne(b => b.Meeting)
				.WithOne(b => b.Proceedings)
				.HasForeignKey<Proceedings>(b => b.MeetingId);
		}
	}
}
