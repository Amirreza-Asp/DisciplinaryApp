using DisciplinarySystem.Domain.Invitations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DisciplinarySystem.Persistence.Mappings.Invitations
{
    public class InvitationUserMappings : IEntityTypeConfiguration<InvitationUser>
    {
        public void Configure(EntityTypeBuilder<InvitationUser> builder)
        {
            builder.HasKey(b => new { b.UserId, b.InvitationId });

            builder.HasOne(b => b.User)
                .WithMany(b => b.InvitationUsers)
                .HasForeignKey(b => b.UserId);

            builder.HasOne(b => b.Invitation)
                .WithMany(b => b.InvitationUsers)
                .HasForeignKey(b => b.InvitationId);
        }
    }
}
