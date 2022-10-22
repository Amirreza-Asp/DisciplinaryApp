using DisciplinarySystem.Domain.Meetings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DisciplinarySystem.Persistence.Mappings.Meetings
{
    public class MeetingMappings : IEntityTypeConfiguration<Meeting>
    {
        public void Configure(EntityTypeBuilder<Meeting> builder)
        {
            builder.HasKey(b => b.Id);
            builder.Property(b => b.Id).ValueGeneratedNever();

            builder.Property(b => b.Title);
            builder.Property(b => b.Description).IsRequired(false);
            builder.Property(b => b.CreateDate).HasDefaultValueSql("GETDATE()");

            builder.OwnsOne(b => b.HoldingTime, b =>
            {
                b.Property(u => u.From).HasColumnName("StartTime");
                b.Property(u => u.To).HasColumnName("EndTime");
            });

        }
    }
}
