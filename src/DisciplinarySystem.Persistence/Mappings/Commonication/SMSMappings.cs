using DisciplinarySystem.Domain.Commonications;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DisciplinarySystem.Persistence.Mappings.Commonication
{
    public class SMSMappings : IEntityTypeConfiguration<SMS>
    {
        public void Configure ( EntityTypeBuilder<SMS> builder )
        {
            builder.HasKey(b => b.Id);
            builder.Property(b => b.Id).ValueGeneratedNever();

            builder.Property(b => b.Text);
            builder.Property(b => b.SendDate);
            builder.Property(b => b.IsDeleted);

            builder.HasQueryFilter(b => b.IsDeleted == false);

            builder.OwnsOne(b => b.PhoneNumber , b =>
            {
                b.Property(b => b.Value).HasColumnName("PhoneNumber").HasMaxLength(13).IsUnicode(false);
            });

            builder.HasOne(b => b.User)
                .WithMany(b => b.SMSCollection)
                .HasForeignKey(b => b.UserId);
        }
    }
}
