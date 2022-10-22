using DisciplinarySystem.Domain.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DisciplinarySystem.Persistence.Mappings.Authentication
{
    public class AuthUserMappings : IEntityTypeConfiguration<AuthUser>
    {
        public void Configure ( EntityTypeBuilder<AuthUser> builder )
        {
            builder.HasKey(b => b.Id);
            builder.Property(b => b.Name);
            builder.Property(b => b.Family);
            builder.Property(b => b.UserName);
            builder.Property(b => b.Password);

            builder.OwnsOne(b => b.PhoneNumber , b =>
            {
                b.Property(u => u.Value).HasColumnName("PhoneNumber").HasMaxLength(11).IsUnicode(false);
            });

            builder.OwnsOne(b => b.NationalCode , b =>
            {
                b.Property(u => u.Value).HasColumnName("NationalCode").HasMaxLength(10).IsUnicode(false);
            });

        }
    }
}
