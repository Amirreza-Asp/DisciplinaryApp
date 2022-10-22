using DisciplinarySystem.Domain.Objections;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DisciplinarySystem.Persistence.Mappings.Objections
{
    public class ObjectionMappings : IEntityTypeConfiguration<Objection>
    {
        public void Configure(EntityTypeBuilder<Objection> builder)
        {
            builder.HasKey(b => b.Id);
            builder.Property(b => b.Id).ValueGeneratedNever();

            builder.Property(b => b.CreateDate).HasDefaultValueSql("GETDATE()");
            builder.Property(b => b.Subject);
            builder.Property(b => b.Result);
            builder.Property(b => b.Description);

            builder.HasOne(b => b.Case)
                .WithMany(b => b.Objections)
                .HasForeignKey(b => b.CaseId);
        }
    }
}
