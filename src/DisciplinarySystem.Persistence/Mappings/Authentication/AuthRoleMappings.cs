using DisciplinarySystem.Domain.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DisciplinarySystem.Persistence.Mappings.Authentication
{
    public class AuthRoleMappings : IEntityTypeConfiguration<AuthRole>
    {
        public void Configure ( EntityTypeBuilder<AuthRole> builder )
        {
            builder.HasKey(x => x.Id);
            builder.Property(b => b.Title);
            builder.Property(b => b.Description).IsRequired(false);
        }
    }
}
