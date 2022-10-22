using DisciplinarySystem.Domain.Informants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DisciplinarySystem.Persistence.Mappings.Informants
{
    public class InvitationDocumentMappings : IEntityTypeConfiguration<InformedDocument>
    {
        public void Configure(EntityTypeBuilder<InformedDocument> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedNever();

            builder.Property(x => x.Name);

            builder.OwnsOne(p => p.File, p =>
            {
                p.Property(x => x.Name).HasColumnName("File").HasMaxLength(DEFAULT_NAME_LENGTH);
            });

            builder.Property(x => x.SendTime).HasDefaultValueSql("GETDATE()");

            builder.HasOne(x => x.Informed)
                .WithMany(x => x.Documents)
                .HasForeignKey(x => x.InformedId);
        }
    }
}
