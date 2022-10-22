using DisciplinarySystem.Domain.RelatedInfos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DisciplinarySystem.Persistence.Mappings.RelatedInfos
{
    public class RelatedInfoDocumentMappings : IEntityTypeConfiguration<RelatedInfoDocument>
    {
        public void Configure(EntityTypeBuilder<RelatedInfoDocument> builder)
        {
            builder.HasKey(b => b.Id);
            builder.Property(b => b.Id).ValueGeneratedNever();

            builder.Property(b => b.SendTime).HasDefaultValueSql("GETDATE()");
            builder.Property(x => x.Name);

            builder.OwnsOne(p => p.File, p =>
            {
                p.Property(x => x.Name).HasColumnName("File").HasMaxLength(DEFAULT_NAME_LENGTH);
            });


            builder.HasOne(x => x.RelatedInfo)
                .WithMany(x => x.Documents)
                .HasForeignKey(x => x.RelatedInfoId);
        }
    }
}
