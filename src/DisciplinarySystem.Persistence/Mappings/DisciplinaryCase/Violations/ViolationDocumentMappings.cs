using DisciplinarySystem.Domain.Violations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DisciplinarySystem.Persistence.Meppings.Violations
{
    public class ViolationDocumentMappings : IEntityTypeConfiguration<ViolationDocument>
    {
        public void Configure(EntityTypeBuilder<ViolationDocument> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedNever();

            builder.Property(x => x.Name);

            builder.OwnsOne(p => p.File, p =>
            {
                p.Property(x => x.Name).HasColumnName("File").HasMaxLength(DEFAULT_NAME_LENGTH);
            });

            builder.Property(x => x.SendTime);

            builder.HasOne(x => x.Violation)
                .WithMany(x => x.Documents)
                .HasForeignKey(x => x.ViolationId);
        }
    }
}
