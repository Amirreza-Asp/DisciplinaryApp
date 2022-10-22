using DisciplinarySystem.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DisciplinarySystem.Persistence.Mappings.Users
{
    public class UserMappings : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(u => u.Id);
            builder.Property(u => u.Id).ValueGeneratedNever();

            builder.Property(b => b.CreateDate).HasDefaultValueSql("GETDATE()");
            builder.Property(b => b.FullName);

            builder.OwnsOne(p => p.NationalCode, p =>
            {
                p.Property(u => u.Value).HasColumnName("NationalCode").HasMaxLength(10);
            });

            builder.OwnsOne(p => p.AttendenceTime, p =>
            {
                p.Property(u => u.From).HasColumnName("StartDate");
                p.Property(u => u.To).HasColumnName("EndDate");
            });

            builder.HasOne(u => u.Role)
                .WithMany(u => u.Users)
                .HasForeignKey(u => u.RoleId);
        }
    }
}
