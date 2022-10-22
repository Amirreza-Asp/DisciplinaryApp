using DisciplinarySystem.Domain.Verdicts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DisciplinarySystem.Persistence.Mappings.Verdicts
{
    public class VerdictMappings : IEntityTypeConfiguration<Verdict>
    {
        public void Configure(EntityTypeBuilder<Verdict> builder)
        {
            builder.HasKey(b => b.Id);

            builder.Property(b => b.Title);
            builder.Property(b => b.Description);
        }
    }
}
