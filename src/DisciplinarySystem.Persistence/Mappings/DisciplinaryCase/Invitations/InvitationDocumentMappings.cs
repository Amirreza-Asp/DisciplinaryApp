using DisciplinarySystem.Domain.Invitations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DisciplinarySystem.Persistence.Mappings.Invitations
{
    public class InvitationDocumentMappings : IEntityTypeConfiguration<InvitationDocument>
    {
        public void Configure(EntityTypeBuilder<InvitationDocument> builder)
        {
            builder.ToTable("InvitationDocument");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedNever();

            builder.Property(x => x.Name);

            builder.OwnsOne(p => p.File, p =>
            {
                p.Property(x => x.Name).HasColumnName("File").HasMaxLength(DEFAULT_NAME_LENGTH);
            });

            builder.Property(x => x.SendTime).HasDefaultValueSql("GETDATE()");

            builder.HasOne(x => x.Invitation)
                .WithMany(x => x.Documents)
                .HasForeignKey(x => x.InvitationId);
        }
    }
}
