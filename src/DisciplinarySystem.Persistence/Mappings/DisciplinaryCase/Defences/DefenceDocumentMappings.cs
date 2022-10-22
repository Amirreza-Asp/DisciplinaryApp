using DisciplinarySystem.Domain.Defences;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DisciplinarySystem.Persistence.Mappings.Defences
{
    public class DefenceDocumentMappings : IEntityTypeConfiguration<DefenceDocument>
    {
        public void Configure(EntityTypeBuilder<DefenceDocument> builder)
        {
            builder.HasKey(b => b.Id);
            builder.Property(b => b.Id).ValueGeneratedNever();

            builder.Property(b => b.SendTime).HasDefaultValueSql("GETDATE()");
            builder.Property(x => x.Name);

            builder.OwnsOne(p => p.File, p =>
            {
                p.Property(x => x.Name).HasColumnName("File").HasMaxLength(DEFAULT_NAME_LENGTH);
            });


            builder.HasOne(x => x.Defence)
                .WithMany(x => x.Documents)
                .HasForeignKey(x => x.DefenceId);
        }
    }
}
