using DisciplinarySystem.Domain.RelatedInfos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DisciplinarySystem.Persistence.Mappings.RelatedInfos
{
    public class RelatedInfoMappings : IEntityTypeConfiguration<RelatedInfo>
    {
        public void Configure(EntityTypeBuilder<RelatedInfo> builder)
        {
            builder.HasKey(b => b.Id);
            builder.Property(b => b.Id).ValueGeneratedNever();

            builder.Property(b => b.CreateDate).HasDefaultValueSql("GETDATE()");
            builder.Property(b => b.Subject);
            builder.Property(b => b.Description);

            builder.HasOne(b => b.Case)
                .WithMany(b => b.RelatedInfos)
                .HasForeignKey(b => b.CaseId);
        }
    }
}
