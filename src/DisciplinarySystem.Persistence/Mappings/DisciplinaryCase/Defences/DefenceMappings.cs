using DisciplinarySystem.Domain.Defences;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DisciplinarySystem.Persistence.Mappings.Defences
{
    public class DefenceMappings : IEntityTypeConfiguration<Defence>
    {
        public void Configure(EntityTypeBuilder<Defence> builder)
        {
            builder.HasKey(u => u.Id);
            builder.Property(u => u.Id).ValueGeneratedNever();

            builder.Property(u => u.Subject);
            builder.Property(u => u.Description);

            builder.Property(u => u.CreateDate).HasDefaultValueSql("GETDATE()");
            builder.Property(u => u.UpdateDate).IsRequired(false);

            builder.HasOne(u => u.Case)
                .WithMany(u => u.Defences)
                .HasForeignKey(u => u.CaseId);
        }
    }
}
