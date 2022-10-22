using DisciplinarySystem.Domain.Informants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DisciplinarySystem.Persistence.Mappings.Informants
{
    public class InformedMappings : IEntityTypeConfiguration<Informed>
    {
        public void Configure(EntityTypeBuilder<Informed> builder)
        {
            builder.HasKey(b => b.Id);
            builder.Property(b => b.Id).ValueGeneratedNever();

            builder.Property(b => b.Subject);
            builder.Property(b => b.CreateDate).HasDefaultValueSql("GETDATE()");
            builder.Property(b => b.FullName);
            builder.Property(b => b.Statements);
            builder.Property(b => b.Father);

            builder.OwnsOne(b => b.NationalCode, b =>
            {
                b.Property(u => u.Value).HasColumnName("NationalCode").HasMaxLength(10);
            });


            builder.OwnsOne(b => b.PhoneNumber, b =>
            {
                b.Property(p => p.Value).HasColumnName("PhoneNumber").HasMaxLength(11);
            });

            builder.HasOne(b => b.Case)
                .WithMany(b => b.Informants)
                .HasForeignKey(b => b.CaseId);
        }
    }
}
