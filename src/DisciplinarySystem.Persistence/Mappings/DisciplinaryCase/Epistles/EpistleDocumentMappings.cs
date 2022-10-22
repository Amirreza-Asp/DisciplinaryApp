using DisciplinarySystem.Domain.Epistles;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DisciplinarySystem.Persistence.Meppings.Epistles
{
    public class EpistleDocumentMappings : IEntityTypeConfiguration<EpistleDocument>
    {

        public void Configure(EntityTypeBuilder<EpistleDocument> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedNever();

            builder.Property(x => x.Name);

            builder.OwnsOne(p => p.File, p =>
            {
                p.Property(x => x.Name).HasColumnName("File").HasMaxLength(DEFAULT_NAME_LENGTH);
            });

            builder.Property(x => x.SendTime).HasDefaultValueSql("GETDATE()");

            builder.HasOne(x => x.Epistle)
                .WithMany(x => x.Documents)
                .HasForeignKey(x => x.EpistleId);
        }
    }
}
