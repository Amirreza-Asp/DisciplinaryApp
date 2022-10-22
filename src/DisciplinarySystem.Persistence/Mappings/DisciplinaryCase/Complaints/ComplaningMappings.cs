using DisciplinarySystem.Domain.Complaints;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DisciplinarySystem.Persistence.Meppings.Complaints
{
    public class ComplaningMappings : IEntityTypeConfiguration<Complaining>
    {
        public void Configure(EntityTypeBuilder<Complaining> builder)
        {
            builder.HasKey(b => b.Id);
            builder.Property(b => b.Id).ValueGeneratedNever();

            builder.Property(b => b.FullName);
            builder.Property(b => b.EducationalGroup).IsRequired(false);
            builder.Property(b => b.Grade).IsRequired(false);
            builder.Property(b => b.College).IsRequired(false);
            builder.Property(b => b.Father).IsRequired(false);

            builder.OwnsOne(b => b.StudentNumber, b =>
            {
                b.Property(u => u.Value).HasColumnName("StudentNumber").HasMaxLength(10);
            });

            builder.OwnsOne(b => b.NationalCode, b =>
            {
                b.Property(u => u.Value).HasColumnName("NationalCode").HasMaxLength(10);
            });

        }
    }
}
