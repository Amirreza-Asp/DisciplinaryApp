using DisciplinarySystem.Domain.Invitations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DisciplinarySystem.Persistence.Mappings.Invitations
{
    public class InvitationMappings : IEntityTypeConfiguration<Invitation>
    {
        public void Configure(EntityTypeBuilder<Invitation> builder)
        {
            builder.HasKey(b => b.Id);
            builder.Property(b => b.Id).ValueGeneratedNever();

            builder.Property(b => b.Subject);
            builder.Property(b => b.CreateDate).HasDefaultValueSql("GetDate()");
            builder.Property(b => b.UpdateDate).IsRequired(false);
            builder.Property(b => b.Description);
            builder.Property(b => b.InviteDate);

            builder.HasOne(b => b.Plaintiff)
                .WithMany(b => b.Invitations)
                .HasForeignKey(b => b.PlaintiffId);
            builder.Property(b => b.PlaintiffId).IsRequired(false);

            builder.HasOne(b => b.Complaining)
                .WithMany(b => b.Invitations)
                .HasForeignKey(b => b.ComplainingId);
            builder.Property(b => b.ComplainingId).IsRequired(false);

            builder.HasOne(b => b.Case)
                .WithMany(b => b.Invitations)
                .HasForeignKey(b => b.CaseId);
        }
    }
}
