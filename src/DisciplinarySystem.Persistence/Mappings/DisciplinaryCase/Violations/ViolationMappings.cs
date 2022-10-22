using DisciplinarySystem.Domain.Violations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DisciplinarySystem.Persistence.Meppings.Violations
{
    public class ViolationMappings : IEntityTypeConfiguration<Violation>
    {
        public void Configure(EntityTypeBuilder<Violation> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedNever();

            builder.Property(x => x.Title);
            builder.Property(x => x.Definition);
            builder.Property(b => b.CreateDate).HasDefaultValueSql("GETDATE()");
            builder.Property(b => b.Vote);

            builder.Property(b => b.UpdateDate).IsRequired(false);

            builder.HasOne(x => x.Category)
                .WithMany(x => x.Violations)
                .HasForeignKey(x => x.CategoryId);

            builder.HasOne(b => b.Case)
                .WithMany(b => b.Violations)
                .HasForeignKey(b => b.CaseId);
        }
    }
}
