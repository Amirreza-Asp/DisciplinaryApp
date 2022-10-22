using DisciplinarySystem.Domain.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DisciplinarySystem.Persistence.Mappings.Authentication
{
    public class UserRoleMappings : IEntityTypeConfiguration<UserRole>
    {
        public void Configure ( EntityTypeBuilder<UserRole> builder )
        {
            builder.HasKey(b => new { b.RoleId , b.UserId });

            builder.HasOne(b => b.User)
                .WithMany(b => b.Roles)
                .HasForeignKey(b => b.UserId);

            builder.HasOne(b => b.Role)
                .WithMany(b => b.Users)
                .HasForeignKey(b => b.UserId);
        }
    }
}
