using DisciplinarySystem.Domain.Complaints;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DisciplinarySystem.Persistence.Meppings.Complaints
{
    internal class PlaintiffMappings : IEntityTypeConfiguration<Plaintiff>
    {
        public void Configure(EntityTypeBuilder<Plaintiff> builder)
        {
            builder.HasKey(u => u.Id);
            builder.Property(u => u.Id).ValueGeneratedNever();

            builder.Property(u => u.FullName);
            builder.Property(u => u.Address);

            builder.OwnsOne(p => p.NationalCode, p =>
            {
                p.Property(u => u.Value).HasColumnName("NationalCode").HasMaxLength(10);
            });
            builder.OwnsOne(p => p.PhoneNumber, p =>
            {
                p.Property(u => u.Value).HasColumnName("PhoneNumber").HasMaxLength(15);
            });
        }
    }
}
