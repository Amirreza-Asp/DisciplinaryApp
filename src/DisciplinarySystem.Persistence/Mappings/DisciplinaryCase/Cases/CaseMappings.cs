using DisciplinarySystem.Domain.DisciplinaryCase.Cases;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DisciplinarySystem.Persistence.Mappings.DisciplinaryCases.Cases
{
    public class CaseMappings : IEntityTypeConfiguration<Case>
    {
        public void Configure(EntityTypeBuilder<Case> builder)
        {
            builder.HasKey(b => b.Id);

            builder.Property(b => b.Status);

            builder.HasOne(b => b.Complaint)
                .WithOne(b => b.Case)
                .HasForeignKey<Case>(b => b.ComplaintId);
        }
    }
}
