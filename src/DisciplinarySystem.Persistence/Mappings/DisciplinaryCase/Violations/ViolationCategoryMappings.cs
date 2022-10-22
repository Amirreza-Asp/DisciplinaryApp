using DisciplinarySystem.Domain.Violations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DisciplinarySystem.Persistence.Meppings.Violations
{
    public class ViolationCategoryMappings : IEntityTypeConfiguration<ViolationCategory>
    {
        public void Configure(EntityTypeBuilder<ViolationCategory> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedNever();

            builder.Property(x => x.Title);
            builder.Property(x => x.Description);
        }
    }
}
