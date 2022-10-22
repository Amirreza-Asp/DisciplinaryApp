using DisciplinarySystem.Domain.Meetings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DisciplinarySystem.Persistence.Mappings.Meetings
{
    public class MeetingUsersMappings : IEntityTypeConfiguration<MeetingUsers>
    {
        public void Configure(EntityTypeBuilder<MeetingUsers> builder)
        {
            builder.HasKey(b => new { b.UserId, b.MeetingId });

            builder.HasOne(b => b.User)
                .WithMany(b => b.MeetingUsers)
                .HasForeignKey(b => b.UserId);

            builder.HasOne(b => b.Meeting)
                .WithMany(b => b.MeetingUsers)
                .HasForeignKey(b => b.MeetingId);
        }
    }
}
