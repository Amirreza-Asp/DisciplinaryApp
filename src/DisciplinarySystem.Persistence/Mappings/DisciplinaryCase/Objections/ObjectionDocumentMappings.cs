using DisciplinarySystem.Domain.Objections;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DisciplinarySystem.Persistence.Mappings.Objections
{
    public class ObjectionDocumentMappings : IEntityTypeConfiguration<ObjectionDocument>
    {
        public void Configure(EntityTypeBuilder<ObjectionDocument> builder)
        {
            builder.ToTable("ObjectionDocuments");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedNever();

            builder.Property(x => x.Name);

            builder.OwnsOne(p => p.File, p =>
            {
                p.Property(x => x.Name).HasColumnName("File").HasMaxLength(DEFAULT_NAME_LENGTH);
            });

            builder.Property(x => x.SendTime).HasDefaultValueSql("GETDATE()");

            builder.HasOne(x => x.Objection)
                .WithMany(x => x.Documents)
                .HasForeignKey(x => x.ObjectionId);
        }
    }
}
